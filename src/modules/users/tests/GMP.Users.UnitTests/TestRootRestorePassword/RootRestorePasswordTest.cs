using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersRootServices.CommandRestorePasswordUser;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventRestorePasswordUser;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestRootRestorePassword
{
    [TestClass]
    public class RootRestorePasswordTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;
        private readonly UsersFactories factory;

        public RootRestorePasswordTest()
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
        /// Test para el flujo normal del caso de uso restablecer la contraseña
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestRestorePasswordUser()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToRestorePassword = Guid.NewGuid();


            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.root, token, idUser);
            User userToEdit = (User)factory.BuildUser("Jose", "Avila", "234234234234", "jose@gmail.com", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.interno, token, idUserToRestorePassword);
            User userUpdated = (User)factory.BuildUser("Jose", "Avila", "234234234234", "jose@gmail.com", encriptedPassword, "carrera 1 este #33-34", TipoUsuarioEnum.interno, id:idUserToRestorePassword);

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Id = userToEdit.ToString(),
                Mail = "juan@gmail.com",
                Name = "Jose",
                Lastname = "Avila",
                TipoUsuario = (int)TipoUsuarioEnum.externo,
                Token = "fsdfsdf345345ret"
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.MailUserRestorePassword))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.MailUserRestorePassword, cancelationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewPassword))
                .Returns(encriptedPassword)
                .Verifiable();

            utilities.Setup(x => x.CreateId(userToEdit.Id))
                .Returns(idUserToRestorePassword)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(userToEdit.Name, userToEdit.Lastname, userToEdit.Phone, userToEdit.Mail, encriptedPassword, userToEdit.Address, userToEdit.TipoUsuario, null, idUserToRestorePassword))
                .Returns(userUpdated)
                .Verifiable();

            userRepository.Setup(x => x.Update<User>(userUpdated, cancelationToken))
                .ReturnsAsync(userUpdated)
                .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(userUpdated))
                .Returns(Task.CompletedTask)
                .Verifiable();

            mapping.Setup(x => x.Map<User, RestorePasswordUserEvent>(userUpdated))
               .Returns(restorePasswordUserEvent)
               .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

           await restorePasswordUser.Handle(request, cancelationToken);

            Assert.IsNull(userUpdated.Token);
        }

        /// <summary>
        /// Se comprueba la excepcion cuando el request esta nulo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestRestorePasswordUser_CommandRequestNullException()
        {
            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando el usuario root no esta registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestRestorePasswordUser_UserNotRegisterException()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(false)
              .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando retorna nulo al recuperar el usuario root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestRestorePasswordUser_EntityNotRecoveredException()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var idUser = Guid.NewGuid();


            User userActual = null;
           
            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(userActual)
                .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando el usuario que realiza la peticion no es root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestRestorePasswordUser_NotRootException()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToRestorePassword = Guid.NewGuid();


            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando retorna nulo al obtener al usuario el cual se restablecera la contraseña
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestRestorePasswordUser_EntityNotRecoveredException_UserToRestorePassword()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToRestorePassword = Guid.NewGuid();


            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.root, token, idUser);
            User userToEdit = null;

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.MailUserRestorePassword))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.MailUserRestorePassword, cancelationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(request, new CancellationToken());
        }
        
        /// <summary>
        /// Se comprueba la excepcion cuando el usuario a restablecer la contraseña no se encuentra registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestRestorePasswordUser_UserNotRegisterException_User_Restore_Password()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToRestorePassword = Guid.NewGuid();


            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.root, token, idUser);

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.MailUserRestorePassword))
              .Returns(false)
              .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando el usuario a restablecer la contraseña es de tipo root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(TypeUserRootException))]
        public async Task TestRestorePasswordUser_TypeUserRootException()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToRestorePassword = Guid.NewGuid();


            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.root, token, idUser);
            User userToEdit = (User)factory.BuildUser("ROOT", "ROOT", "234234234234", "jose@gmail.com", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.root, token, idUserToRestorePassword);

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.MailUserRestorePassword))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.MailUserRestorePassword, cancelationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await restorePasswordUser.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la exepcion cuando no se puede actualizar la entidad en la db
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(SaveNullException))]
        public async Task TestRestorePasswordUser_SaveNullException()
        {
            var request = new RestorePasswordUserCommand
            {
                RootClaims = new List<Claim> { },
                MailUserRestorePassword = "cristian.cubides@uptc.edu.co",
                NewPassword = "sdfsdf23423yhrsh3453areassd"
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToRestorePassword = Guid.NewGuid();


            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.root, token, idUser);
            User userToEdit = (User)factory.BuildUser("Jose", "Avila", "234234234234", "jose@gmail.com", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.interno, token, idUserToRestorePassword);
            User userUpdated = null;

            RestorePasswordUserEvent restorePasswordUserEvent = new RestorePasswordUserEvent
            {
                Name = "Jose",
            };

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.MailUserRestorePassword))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == request.MailUserRestorePassword, cancelationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewPassword))
                .Returns(encriptedPassword)
                .Verifiable();

            utilities.Setup(x => x.CreateId(userToEdit.Id))
                .Returns(idUserToRestorePassword)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(userToEdit.Name, userToEdit.Lastname, userToEdit.Phone, userToEdit.Mail, encriptedPassword, userToEdit.Address, userToEdit.TipoUsuario, userToEdit.Token, idUserToRestorePassword))
                .Returns(userUpdated)
                .Verifiable();

            userRepository.Setup(x => x.Update<User>(userUpdated, cancelationToken))
                .ReturnsAsync(userUpdated)
                .Verifiable();


            var restorePasswordUser = new RestorePasswordUserHandler(userRepository.Object, userSecurity.Object, userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

           await restorePasswordUser.Handle(request, cancelationToken);

        }

    }
}
