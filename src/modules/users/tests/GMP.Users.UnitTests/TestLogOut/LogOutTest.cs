using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersServices.CommandLogOut;
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

namespace GMP.Users.UnitTests.TestLogOut
{
    [TestClass]
    public class LogOutTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly UsersFactories factory;

        public LogOutTest()
        {
            userFactory = new Mock<IUsersFactories>();
            userRepository = new Mock<IUsersRepositories>();
            utilities = new Mock<IUtilities>();
            mapping = new Mock<IAutoMapping>();
            eventPublisher = new Mock<IEventPublisher>();
            userSecurity = new Mock<IUserSecurity>();
            factory = new UsersFactories();
        }

        [TestMethod]
        public async Task TestLogOutHandler()
        {
            var request = new LogOutUserCommand
            {
            };
            var cancellationToken = new CancellationToken();
            Guid id = new Guid();
            User user = (User)factory.BuildUser("Julian", "Cubides", "3138989123", "cristian.cubides@gmail.com", "sdfsdfg34345", "Diagonal 5", TipoUsuarioEnum.interno, "sfsdfsdfdfhd", id);
            User userUpdate = (User)factory.BuildUser("Julian", "Cubides", "3138989123", "cristian.cubides@gmail.com", "sdfsdfg34345", "Diagonal 5",  TipoUsuarioEnum.interno, id:id);

            var logOutEvent = new LogOutEvent()
            {

            };
            var mailUser = "cristian.cubides@gmail.com";

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(user)
                .Verifiable();

            utilities.Setup(x => x.CreateId())
                .Returns(id)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(user.Name, user.Lastname, user.Phone, user.Mail, user.Password, user.Address, TipoUsuarioEnum.interno, null, Guid.Parse(user.Id)))
             .Returns(userUpdate)
             .Verifiable();

            userRepository.Setup(x => x.Update(userUpdate, cancellationToken))
                 .ReturnsAsync(userUpdate)
                 .Verifiable();

            mapping.Setup(x => x.Map<User, LogOutEvent>(userUpdate))
                .Returns(logOutEvent)
              .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(logOutEvent))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var logOutUserHandler = new LogOutUserHandler(userRepository.Object, utilities.Object,
              userFactory.Object, mapping.Object, userSecurity.Object, eventPublisher.Object);

            await logOutUserHandler.Handle(request, new CancellationToken());

            Assert.IsNull(userUpdate.Token);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando la peticion es nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestLogOutUser_CommandRequestNullException()
        {
            var logOutUserHandler = new LogOutUserHandler(userRepository.Object, utilities.Object,
              userFactory.Object, mapping.Object, userSecurity.Object, eventPublisher.Object);

            await logOutUserHandler.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando el usuario no se puede obtener
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestLogOutUser_EntityNotRecoveredException()
        {
            var request = new LogOutUserCommand
            {
            };
            var mailUser = "cristian.cubides@gmail.com";
            var cancelationToken = new CancellationToken();
            User user = null;

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(user)
                .Verifiable();

            var logOutUserHandler = new LogOutUserHandler(userRepository.Object, utilities.Object,
              userFactory.Object, mapping.Object, userSecurity.Object, eventPublisher.Object);

            await logOutUserHandler.Handle(request, cancelationToken);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando retorna nulo al actualizar el usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UpdateNullEntityException))]
        public async Task TestLogOutUser_UpdateNullEntityException()
        {
            var request = new LogOutUserCommand
            {
            };
            var mailUser = "cristian.cubides@gmail.com";
            var cancellationToken = new CancellationToken();
            Guid id = new Guid();
            User user = (User)factory.BuildUser("Julian", "Cubides", "3138989123", "cristian.cubides@gmail.com", "sdfsdfg34345", "Diagonal 5", TipoUsuarioEnum.interno, "sfsdfsdfdfhd", id);
            User userUpdate = (User)factory.BuildUser("Julian", "Cubides", "3138989123", "cristian.cubides@gmail.com", "sdfsdfg34345", "Diagonal 5", TipoUsuarioEnum.interno, id:id);
            User userUpdateAux = null;

            var logOutEvent = new LogOutEvent()
            {

            };

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
               .ReturnsAsync(user)
               .Verifiable();

            utilities.Setup(x => x.CreateId())
                .Returns(id)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(user.Name, user.Lastname, user.Phone, user.Mail, user.Password, user.Address, TipoUsuarioEnum.interno, null, Guid.Parse(user.Id)))
             .Returns(userUpdate)
             .Verifiable();

            userRepository.Setup(x => x.Update(userUpdate, cancellationToken))
                 .ReturnsAsync(userUpdateAux)
                 .Verifiable();

            var logOutUserHandler = new LogOutUserHandler(userRepository.Object, utilities.Object,
              userFactory.Object, mapping.Object, userSecurity.Object, eventPublisher.Object);

            await logOutUserHandler.Handle(request, cancellationToken);
        }

    }
}
