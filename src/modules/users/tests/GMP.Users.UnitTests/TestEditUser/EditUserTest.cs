using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersServices.CommadEditUser;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventEditUser;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestEditUser
{
    [TestClass]
    public class EditUserTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;
        private readonly UsersFactories factory;

        public EditUserTest()
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
        /// Flujo normal para el caso de uso editar usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestEditUserHandler()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.interno
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);
            User userToEdit = (User)factory.BuildUser("Jose", "Avila", "234234234234", "jose@gmail.com", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.interno, "sdfsdf345345ttyj56756", idUserToEdit);
            User userEdited = (User)factory.BuildUser(request.DataUserToEdit.Name, request.DataUserToEdit.LastName, request.DataUserToEdit.Phone, request.DataUserToEdit.Mail, encriptedPassword, request.DataUserToEdit.Address, request.DataUserToEdit.TipoUsuario, id:idUserEdited);

            EventEditUser eventEditUser = new EventEditUser
            {
                Id = idUserEdited.ToString(),
                Mail = "juan@gmail.com",
                Name = "Jose",
                Lastname = "Avila",
                TipoUsuario = (int)TipoUsuarioEnum.externo,
                Token = "fsdfsdf345345ret"
            };

            EditUserDTO editUserDTO = new EditUserDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhian@gmail.com",
                Address = "Carrera 1 Este #33-34"
            };

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancelationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(false)
             .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Id == request.UserIdToEdit, cancelationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Id == request.UserIdToEdit))
            .Returns(true)
            .Verifiable();

            utilities.Setup(x => x.CreateId(userToEdit.Id))
               .Returns(idUserEdited)
               .Verifiable();

            userFactory.Setup(x => x.BuildUser(request.DataUserToEdit.Name, request.DataUserToEdit.LastName, request.DataUserToEdit.Phone, request.DataUserToEdit.Mail, userToEdit.Password, request.DataUserToEdit.Address, request.DataUserToEdit.TipoUsuario, null, idUserEdited))
                .Returns(userEdited)
                .Verifiable();

            userRepository.Setup(x => x.Update<User>(userEdited, cancelationToken))
                .ReturnsAsync(userEdited)
                .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(userEdited))
                .Returns(Task.CompletedTask)
                .Verifiable();

            mapping.Setup(x => x.Map<User, EventEditUser>(userEdited))
               .Returns(eventEditUser)
               .Verifiable();

            mapping.Setup(x => x.Map<User, EditUserDTO>(userEdited))
               .Returns(editUserDTO)
               .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                userRepository.Object, utilities.Object);

            EditUserDTO objectDTO = await editUserHandler.Handle(request, cancelationToken);

            Assert.AreEqual(editUserDTO, objectDTO);
        }

        /// <summary>
        /// Se comprueba la excepcion cuando la peticion esta nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestEditUser_CommandRequestNullException()
        {
            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                userRepository.Object, utilities.Object);

            await editUserHandler.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando el usuario que realiza la peticion no esta registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestEditUser_UserNotRegisterException()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(false)
              .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando se retorna nulo al recuperar al cliente
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestEditUser_EntityNotRecoveredException()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";

            User userActual = null;

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(false)
             .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(userActual)
                .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando un usuario externo realiza la peticion, sin embargo este usuario no tiene permisos para realizar la accion
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(AccesNotAutorizedUser))]
        public async Task TestEditUser_AccesNotAutorizedExternalUser()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";
            var name = "Julian";
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.externo, token, idUser);

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(false)
             .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(userActual)
                .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando el usuario a editar no esta registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestEditUser_UserNotRegisterException_EditUser()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";
            var name = "Julian";
            var lastName = "Cubides";
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(false)
             .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Id == request.UserIdToEdit))
            .Returns(false)
            .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        ///  Se comprueba la excepcion cuando retorna nulo al obtnener el usuario que se va a editar
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestEditUser_EntityNotRecoveredException_EditUser()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";
            var name = "Julian";
            var lastName = "Cubides";
            var cancellationToken = new CancellationToken();
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);
            User userToEdit = null;

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(false)
             .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Id == request.UserIdToEdit))
            .Returns(true)
            .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Id == request.UserIdToEdit, cancellationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento cuando el tipo de usuario es root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(TypeUserRootException))]
        public async Task TestEditUser_TypeUserRootException()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.root
                }
            };

            string mailUser = "julian4@gmail.com";
            var name = "Julian";
            var lastName = "Cubides";
            var cancellationToken = new CancellationToken();
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);
            User userToEdit = (User)factory.BuildUser("Jose", "Avila", "234234234234", "cristian.cubides@uptc.edu.co", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.interno, "sdfsdf345345ttyj56756", idUserToEdit);

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Id == request.UserIdToEdit))
            .Returns(true)
            .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Id == request.UserIdToEdit, cancellationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepcion cuando el correo que se edita ya se encuentra asignado a otro usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserExistsException))]
        public async Task TestEditUser_UserExistsException()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";
            var name = "Julian";
            var lastName = "Cubides";
            var cancellationToken = new CancellationToken();
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);
            User userToEdit = (User)factory.BuildUser("Jose", "Avila", "234234234234", "jose@gmail.com", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.interno, "sdfsdf345345ttyj56756", idUserToEdit);

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Id == request.UserIdToEdit))
            .Returns(true)
            .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Id == request.UserIdToEdit, cancellationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(true)
             .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba la excepcion cuando retorna nulo al actualizar el cliente
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UpdateNullEntityException))]
        public async Task TestEditUser_UpdateNullEntityException()
        {
            var request = new EditUserCommand
            {
                UserClaims = new List<System.Security.Claims.Claim>
                {

                },

                UserIdToEdit = "50f51b30-a2dd-4c4f-8229-e6d054c711b9",

                DataUserToEdit = new EditUserCommand.DataUserEdit()
                {
                    Name = "julian",
                    LastName = "Cubides",
                    Phone = "3138989123",
                    Address = "Carrera 1 este #33-34",
                    Mail = "cristian.cubides@uptc.edu.co",
                    TipoUsuario = TipoUsuarioEnum.externo
                }
            };

            string mailUser = "julian4@gmail.com";
            string mailUserData = "cristian.cubides@uptc.edu.co";
            var name = "Julian";
            var lastName = "Cubides";
            var cancellationToken = new CancellationToken();
            var phone = "3138989123";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUser = Guid.NewGuid();
            var idUserToEdit = Guid.NewGuid();
            var idUserEdited = Guid.NewGuid();

            User userActual = (User)factory.BuildUser(name, lastName, phone, mailUser, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUser);
            User userToEdit = (User)factory.BuildUser("Jose", "Avila", "234234234234", "jose@gmail.com", "fsdfsdf345345ret", "carrera 1 este #33-34", TipoUsuarioEnum.interno, "sdfsdf345345ttyj56756", idUserToEdit);
            User userEdited = null;

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserData))
             .Returns(false)
             .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUser, new CancellationToken()))
                .ReturnsAsync(userActual)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Id == request.UserIdToEdit))
            .Returns(true)
            .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Id == request.UserIdToEdit, cancellationToken))
               .ReturnsAsync(userToEdit)
               .Verifiable();

            utilities.Setup(x => x.CreateId(userToEdit.Id))
               .Returns(idUserEdited)
               .Verifiable();

            userFactory.Setup(x => x.BuildUser(request.DataUserToEdit.Name, request.DataUserToEdit.LastName, request.DataUserToEdit.Phone, request.DataUserToEdit.Mail, encriptedPassword, request.DataUserToEdit.Address,  TipoUsuarioEnum.interno, userToEdit.Token, idUserEdited))
                .Returns(userEdited)
                .Verifiable();

            userRepository.Setup(x => x.Update<User>(userEdited, cancellationToken))
                .ReturnsAsync(userEdited)
                .Verifiable();

            var editUserHandler = new EditUserHandler(eventPublisher.Object, mapping.Object, userFactory.Object, userSecurity.Object,
                   userRepository.Object, utilities.Object);

            await editUserHandler.Handle(request, new CancellationToken());
        }
    }
}
