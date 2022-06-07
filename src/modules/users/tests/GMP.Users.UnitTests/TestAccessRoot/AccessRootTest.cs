using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersRootServices.CommandAccessRoot;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventAccessRoot;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestAccessRoot
{
    [TestClass]
    public class AccessRootTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;

        private readonly UsersFactories factory;

        public AccessRootTest()
        {
            userFactory = new Mock<IUsersFactories>();
            userRepository = new Mock<IUsersRepositories>();
            userSecurity = new Mock<IUserSecurity>();
            utilities = new Mock<IUtilities>();
            mapping = new Mock<IAutoMapping>();
            eventPublisher = new Mock<IEventPublisher>();

            factory = new UsersFactories();
        }

        /// <summary>
        /// caso cuando el usuario root ya esta registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestAccessRootHandler()
        {
            var request = new AccessRootCommand
            {
                Mail = "william4@gmail.com",
                Password = "Carbaval123"
            };
            var cancellationToken = new CancellationToken();

            var mailUserRoot = "william4@gmail.com";
            var passwordUserRoot = "Carbaval123";
            var encrytPassword = "qwe23e3dd";
            var encrytPasswordRequest = "qwe23e3dd";
            Guid id = Guid.NewGuid();
            string token = "jsjnssu383sjsj8";

            var userRoot = (User)this.factory.BuilRegisterUserRoot(
                    mail: request.Mail,
                    password: encrytPassword,
                    token: token,
                    id: id);

            var accessRootDTO = new AccessRootDTO
            {
                Token = token
            };

            var eventRoot = new AccessRootEvent
            {
                Id = id.ToString(),
                Mail = mailUserRoot,
                TipoUsuario = 2
            };

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(passwordUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(passwordUserRoot))
                .Returns(encrytPassword)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.Password))
                .Returns(encrytPasswordRequest)
                .Verifiable();

            utilities.Setup(x => x.CreateId())
                .Returns(id)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.Mail, cancellationToken))
                .ReturnsAsync(userRoot)
                .Verifiable();

            userSecurity.Setup(x => x.CreateToken(id, mailUserRoot, mailUserRoot,
                TipoUsuarioEnum.root.ToString()))
                .Returns(token)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.Password))
               .Returns(encrytPassword)
               .Verifiable();

            userFactory.Setup(x => x.BuilRegisterUserRoot(request.Mail, encrytPassword, token, id))
                .Returns(userRoot)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
                .Returns(true)
                .Verifiable();

            userRepository.Setup(x => x.Delete<User>(userRoot, cancellationToken))
                .Verifiable();

            userRepository.Setup(x => x.Save<User>(userRoot, cancellationToken))
                .ReturnsAsync(userRoot)
                .Verifiable();

            mapping.Setup(x => x.Map<User, AccessRootEvent>(userRoot))
                .Returns(eventRoot)
                .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(eventRoot))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var AccessRootHandler = new AccessRootHandler(userRepository.Object, userSecurity.Object, 
                userFactory.Object, utilities.Object, eventPublisher.Object, mapping.Object);
            var dto = await AccessRootHandler.Handle(request, cancellationToken);

            Assert.AreEqual(dto.Token, accessRootDTO.Token);
        }

        /// <summary>
        /// cuando la peticion es nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestAccessRootHandler_CommandRequestNullException()
        {
            var cancellationToken = new CancellationToken();

            var AccessRootHandler = new AccessRootHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, eventPublisher.Object, mapping.Object);

            await AccessRootHandler.Handle(null, cancellationToken);
        }

        /// <summary>
        /// determina si el mail no corresponde a un usuario root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestAccessRootHandler_NotRootException_mail()
        {
            var request = new AccessRootCommand
            {
                Mail = "william4@gmail.com",
                Password = "Carbaval123"
            };
            var cancellationToken = new CancellationToken();

            var passwordUserRoot = "Carbaval123";
            var encrytPassword = "qwe23e3dd";
            Guid id = Guid.NewGuid();
            string token = "jsjnssu383sjsj8";
            var otroMail = "david4@gmail.com";

            var accessRootDTO = new AccessRootDTO
            {
                Token = token
            };


            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(otroMail)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(passwordUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(passwordUserRoot))
                .Returns(encrytPassword)
                .Verifiable();


            var AccessRootHandler = new AccessRootHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, eventPublisher.Object, mapping.Object);

            await AccessRootHandler.Handle(request, cancellationToken);
        }


        /// <summary>
        /// determina si el password no corresponde a un usuario root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestAccessRootHandler_NotRootException_password()
        {
            var request = new AccessRootCommand
            {
                Mail = "william4@gmail.com",
                Password = "Carbaval123"
            };
            var cancellationToken = new CancellationToken();

            var passwordUserRoot = "Casaswdsde33";
            var encrytPassword = "qwe23e3dd";
            Guid id = Guid.NewGuid();
            string token = "jsjnssu383sjsj8";
            var mail = "william4@gmail.com";
            var otroEncrytPassword = "awe23e3ddsdde3wd";

            var accessRootDTO = new AccessRootDTO
            {
                Token = token
            };


            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(mail)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(passwordUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(passwordUserRoot))
                .Returns(encrytPassword)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.Password))
                .Returns(otroEncrytPassword)
                .Verifiable();


            var AccessRootHandler = new AccessRootHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, eventPublisher.Object, mapping.Object);

            await AccessRootHandler.Handle(request, cancellationToken);
        }

        [ExpectedException(typeof(SaveNullException))]
        [TestMethod]
        public async Task TestAccessRootHandler_SaveNullException()
        {
            var request = new AccessRootCommand
            {
                Mail = "william4@gmail.com",
                Password = "Carbaval123"
            };
            var cancellationToken = new CancellationToken();

            var mailUserRoot = "william4@gmail.com";
            var passwordUserRoot = "Carbaval123";
            var encrytPassword = "qwe23e3dd";
            var encrytPasswordRequest = "qwe23e3dd";
            Guid id = Guid.NewGuid();
            string token = "jsjnssu383sjsj8";

            var userRoot = (User)this.factory.BuilRegisterUserRoot(
                    mail: request.Mail,
                    password: encrytPassword,
                    token: token,
                    id: id);

            var accessRootDTO = new AccessRootDTO
            {
                Token = token
            };

            var eventRoot = new AccessRootEvent
            {
                Id = id.ToString(),
                Mail = mailUserRoot,
                TipoUsuario = 2
            };

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(passwordUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(passwordUserRoot))
                .Returns(encrytPassword)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.Password))
                .Returns(encrytPasswordRequest)
                .Verifiable();

            utilities.Setup(x => x.CreateId())
                .Returns(id)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.Mail, cancellationToken))
                .ReturnsAsync(userRoot)
                .Verifiable();

            userSecurity.Setup(x => x.CreateToken(id, mailUserRoot, mailUserRoot,
                TipoUsuarioEnum.root.ToString()))
                .Returns(token)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.Password))
               .Returns(encrytPassword)
               .Verifiable();

            userFactory.Setup(x => x.BuilRegisterUserRoot(request.Mail, encrytPassword, token, id))
                .Returns(userRoot)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.Mail))
                .Returns(true)
                .Verifiable();

            userRepository.Setup(x => x.Delete<User>(userRoot, cancellationToken))
                .Verifiable();

            userRepository.Setup(x => x.Save<User>(userRoot, cancellationToken))
                .Verifiable();

            var AccessRootHandler = new AccessRootHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, eventPublisher.Object, mapping.Object);

            await AccessRootHandler.Handle(request, cancellationToken);
        }
    }
}
