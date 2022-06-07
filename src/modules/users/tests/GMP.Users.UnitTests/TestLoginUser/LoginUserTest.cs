using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Users.Application.UsersServices.CommandLoginUser;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestLoginUser
{
    [TestClass]
    public class LoginUserTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> autoMapping;
        private readonly Mock<IEventPublisher> eventPublisher;


        private readonly UsersFactories factory;

        public LoginUserTest()
        {
            userFactory = new Mock<IUsersFactories>();
            userRepository = new Mock<IUsersRepositories>();
            userSecurity = new Mock<IUserSecurity>();
            utilities = new Mock<IUtilities>();
            autoMapping = new Mock<IAutoMapping>();
            eventPublisher = new Mock<IEventPublisher>();
            factory = new UsersFactories();
        }

        /// <summary>
        /// Se comprueba el flujo comun para el logeo de un usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestUserLoginHandler()
        {
            var request = new LoginUserCommand
            {
                Mail = "cristhiancubides84@gmail.com",
                Password = "julian10"
            };



            var password = "julian10";
            var mail = "cristhiancubides84@gmail.com";
            var cancelationToken = new CancellationToken();
            var token = "jsusjsuj8983j45.";
            var encryptPassword = "sfasdfwerfsdf";
            var id = Guid.NewGuid();
            var newToken = "jssuisjsuj8983j45.";
            var name = "Julian";
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";

            var user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, token, id);
            var userUpdate = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, newToken, id);

            var loginUserEvent = new LoginUserEvent
            {
                Id = id.ToString(),
                Mail = mail,
                TipoUsuario = 1
            };


            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
                .Returns(true)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.Mail, cancelationToken))
                .ReturnsAsync(user)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(user.Name, user.Lastname, user.Phone, user.Mail, user.Password, user.Address, TipoUsuarioEnum.interno, newToken, Guid.Parse(user.Id)))
              .Returns(userUpdate)
              .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(password))
                .Returns(encryptPassword)
                .Verifiable();

            utilities.Setup(x => x.CreateId(id.ToString()))
                .Returns(id)
                .Verifiable();

            userSecurity.Setup(x => x.CreateToken(id, user.Name, user.Mail, user.TipoUsuario.ToString()))
                .Returns(newToken)
                .Verifiable();

            userRepository.Setup(x => x.Update(userUpdate, cancelationToken))
                .ReturnsAsync(userUpdate)
                .Verifiable();

            autoMapping.Setup(x => x.Map<User, LoginUserEvent>(userUpdate))
                .Returns(loginUserEvent)
                .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(loginUserEvent))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var loginUserHandler = new LoginUserHandler(userRepository.Object, userSecurity.Object,
               userFactory.Object, utilities.Object, eventPublisher.Object, autoMapping.Object);

            LoginUserDTO loginUserDTO = await loginUserHandler.Handle(request, cancelationToken);

            Assert.IsNotNull(loginUserDTO);
            Assert.AreEqual(loginUserDTO.Token, newToken);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando la peticion es nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TesLoginUser_CommandRequestNullException()
        {
            var loginUserHandler = new LoginUserHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, eventPublisher.Object, autoMapping.Object);

            await loginUserHandler.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando el usuario no se encuentra registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestLoginUser_UserNotRegisterException()
        {
            var request = new LoginUserCommand
            {
                Mail = "cristhiancubides84@gmail.com",
                Password = "julian10"
            };

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
                .Returns(false)
                .Verifiable();

            var loginUserHandler = new LoginUserHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, eventPublisher.Object, autoMapping.Object);

            await loginUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando el usuario no se puede obtener
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestLoginUser_EntityNotRecoveredException()
        {
            var request = new LoginUserCommand
            {
                Mail = "cristhiancubides84@gmail.com",
                Password = "julian10"
            };

            var cancelationToken = new CancellationToken();
            User user = null;

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
                .Returns(true)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.Mail, new CancellationToken()))
                .ReturnsAsync(user)
                .Verifiable();

            var loginUserHandler = new LoginUserHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, eventPublisher.Object, autoMapping.Object);

            await loginUserHandler.Handle(request, cancelationToken);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando la contraseña es incorrecta
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(IncorrectPasswordException))]
        public async Task TestLoginUser_IncorrectPasswordException()
        {
            var request = new LoginUserCommand
            {
                Mail = "cristhiancubides84@gmail.com",
                Password = "julian10"
            };

            var password = "julian10";
            var encryptPassword = "sfasdfwerfsdf";

            var cancelationToken = new CancellationToken();

            var user = (User)factory.BuildUser("Cristian", "Cubides", "3138981923", "cristian16@gamil.com", "sdfw324rtt", "Diagonal 5 #4-32", TipoUsuarioEnum.interno, "hfghfgh234234", Guid.NewGuid());
            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
                .Returns(true)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.Mail, cancelationToken))
                .ReturnsAsync(user)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(password))
                .Returns(encryptPassword)
                .Verifiable();

            var loginUserHandler = new LoginUserHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, eventPublisher.Object, autoMapping.Object);

            await loginUserHandler.Handle(request, cancelationToken);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando retorna nulo al actualizar el usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UpdateNullEntityException))]
        public async Task TestLoginUser_UpdateNullEntityException()
        {
            var request = new LoginUserCommand
            {
                Mail = "cristhiancubides84@gmail.com",
                Password = "julian10"
            };

            var password = "julian10";
            var mail = "cristhiancubides84@gmail.com";
            var cancelationToken = new CancellationToken();
            var token = "jsusjsuj8983j45.";
            var encryptPassword = "sfasdfwerfsdf";
            var id = Guid.NewGuid();
            var newToken = "jssuisjsuj8983j45.";
            var name = "Julian";
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";


            var user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, token, id);
            var userUpdate = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, newToken, id);
            User userUpdateAux = null;

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.Mail, cancelationToken))
                .ReturnsAsync(user)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(user.Name, user.Lastname, user.Phone, user.Mail, user.Password, user.Address, TipoUsuarioEnum.interno, newToken, Guid.Parse(user.Id)))
              .Returns(userUpdate)
              .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(password))
                .Returns(encryptPassword)
                .Verifiable();

            utilities.Setup(x => x.CreateId(id.ToString()))
                .Returns(id)
                .Verifiable();

            userSecurity.Setup(x => x.CreateToken(id, user.Name, user.Mail, user.TipoUsuario.ToString()))
                .Returns(newToken)
                .Verifiable();

            userRepository.Setup(x => x.Update(userUpdate, cancelationToken))
                .ReturnsAsync(userUpdateAux)
                .Verifiable();

            var loginUserHandler = new LoginUserHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, eventPublisher.Object, autoMapping.Object);

            await loginUserHandler.Handle(request, cancelationToken);

        }
    }
}
