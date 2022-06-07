using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersServices.CommandInternalRegisterExternal;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestInternalRegisterExternal
{
    [TestClass]
    public class InternalRegisterExternalTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;
        private readonly UsersFactories factory;

        public InternalRegisterExternalTest()
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
        /// Se comprube el flujo basico del caso de uso registrar usuario externo por interno
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestInternalRegisterUserExternal()
        {
            var request = new InternalRegisterExternalCommand
            {
                UserInternalClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new InternalRegisterExternalCommand.NewUser
                {
                    Name = "Jose",
                    Lastname = "Avila",
                    Phone = "313456789",
                    Mail = "juan@gmail.com",
                    Password = "sdfsdf32",
                    Address = "Diagonal 4-34",
                }
            };

            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            string encriptedPassword = "sadfsdi3242034sdfasdf234";
            var idUserInternal = Guid.NewGuid();
            var idUserExternal = Guid.NewGuid();

            User userInternal = (User)factory.BuildUser(name, lastName, phone, mail, encriptedPassword, address, TipoUsuarioEnum.interno, token, idUserInternal);

            User userExternal = (User)factory.BuildUser("Jose", "Avila", "313456789", "juan@gmail.com", encriptedPassword, "Diagonal 4-34", TipoUsuarioEnum.externo, "fsdfsdfs423", idUserExternal);

            InternalRegisterExternalEvent internalRegisterExternalEvent = new InternalRegisterExternalEvent()
            {
                Id = idUserExternal.ToString(),
                Name = "Jose",
                Lastname = "Avila",
                Mail = "juan@gmail.com",
                Token = "fsdfsdfs423",
                TipoUsuario = (int)TipoUsuarioEnum.externo
            };

            InternalRegisterExternalDTO internalRegisterExternalDTO = new InternalRegisterExternalDTO
            {
                Name = "Jose",
                Lastname = "Avila",
                Phone = "313456789",
                Mail = "juan@gmail.com",
                Password = "sdfsdf32",
                Address = "Diagonal 4-34",
                Token = "fsdfsdfs423"
            };

            var mailUserInternal = "cristhiancubides10@gmail.com";


            userSecurity.Setup(x => x.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL))
                .Returns(mailUserInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserInternal))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUserInternal, cancelationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserExternal.Mail))
              .Returns(false)
              .Verifiable();

            utilities.Setup(x => x.CreateId())
                .Returns(idUserExternal)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserExternal.Password))
             .Returns(encriptedPassword)
             .Verifiable();

            userFactory.Setup(x => x.BuildUser(userExternal.Name, userExternal.Lastname, userExternal.Phone, userExternal.Mail,
                userExternal.Password, userExternal.Address, userExternal.TipoUsuario, null, idUserExternal))
                .Returns(userExternal)
                .Verifiable();

            userRepository.Setup(x => x.Save<User>(userExternal, cancelationToken))
                .ReturnsAsync(userExternal)
                .Verifiable();

            mapping.Setup(x => x.Map<User, InternalRegisterExternalEvent>(userExternal))
                .Returns(internalRegisterExternalEvent)
                .Verifiable();

         

            eventPublisher.Setup(x => x.PublishEventAsync(internalRegisterExternalEvent))
                .Returns(Task.CompletedTask)
                .Verifiable();

            mapping.Setup(x => x.Map<User, InternalRegisterExternalDTO>(userExternal))
                .Returns(internalRegisterExternalDTO)
                .Verifiable();

            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            InternalRegisterExternalDTO objectDTO = await internalRegisterExternalHandler.Handle(request, cancelationToken);

            Assert.AreEqual(internalRegisterExternalDTO, objectDTO);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando la peticion es nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestInternalRegisterExternal_CommandRequestNullException()
        {
            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await internalRegisterExternalHandler.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando el usuario no se encuentra registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestInternalRegisterExternal_UserNotRegisterException()
        {
            var request = new InternalRegisterExternalCommand
            {
                UserInternalClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new InternalRegisterExternalCommand.NewUser
                {
                    Name = "Jose",
                    Lastname = "Avila",
                    Phone = "313456789",
                    Mail = "juan@gmail.com",
                    Password = "sdfsdf32",
                    Address = "Diagonal 4-34",
                }
            };

            var mailUserInternal = "cristhian10@gmail.com";

            userSecurity.Setup(x => x.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL))
                .Returns(mailUserInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserInternal))
              .Returns(false)
              .Verifiable();

            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await internalRegisterExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el caso cuando no es un usuario interno
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(InternalUserException))]
        public async Task TestInternalRegisterExternal_InternalUserException()
        {
            var request = new InternalRegisterExternalCommand
            {
                UserInternalClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new InternalRegisterExternalCommand.NewUser
                {
                    Name = "Jose",
                    Lastname = "Avila",
                    Phone = "313456789",
                    Mail = "juan@gmail.com",
                    Password = "sdfsdf32",
                    Address = "Diagonal 4-34",
                }
            };
            var password = "julian10";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var idUserInternal = Guid.NewGuid();
            var idUserExternal = Guid.NewGuid();

            User userInternal = (User)factory.BuildUser(name, lastName, phone, mail, password, address, TipoUsuarioEnum.externo, token, idUserInternal);

            var mailUserInternal = "cristhian10@gmail.com";

            userSecurity.Setup(x => x.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL))
                .Returns(mailUserInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserInternal))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUserInternal, cancelationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await internalRegisterExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el caso cuando retona null al recuperar el usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestInternalRegisterExternal_EntityNotRecoveredException()
        {
            var request = new InternalRegisterExternalCommand
            {
                UserInternalClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new InternalRegisterExternalCommand.NewUser
                {
                    Name = "Jose",
                    Lastname = "Avila",
                    Phone = "313456789",
                    Mail = "juan@gmail.com",
                    Password = "sdfsdf32",
                    Address = "Diagonal 4-34",
                }
            };
            var cancelationToken = new CancellationToken(); ;
            var idUserInternal = Guid.NewGuid();
            var idUserExternal = Guid.NewGuid();

            User userInternal = null;

            var mailUserInternal = "cristhian10@gmail.com";

            userSecurity.Setup(x => x.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL))
                .Returns(mailUserInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserInternal))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUserInternal, cancelationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await internalRegisterExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el caso cuando el usuario ya se encuentra registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserExistsException))]
        public async Task TestInternalRegisterExternal_UserExistsException()
        {
            var request = new InternalRegisterExternalCommand
            {
                UserInternalClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new InternalRegisterExternalCommand.NewUser
                {
                    Name = "Jose",
                    Lastname = "Avila",
                    Phone = "313456789",
                    Mail = "juan@gmail.com",
                    Password = "sdfsdf32",
                    Address = "Diagonal 4-34",
                }
            };

            var password = "julian10";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var idUserInternal = Guid.NewGuid();

            User userInternal = (User)factory.BuildUser(name, lastName, phone, mail, password, address, TipoUsuarioEnum.interno, token, idUserInternal);

            var mailUserInternal = "cristhian10@gmail.com";

            userSecurity.Setup(x => x.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL))
                .Returns(mailUserInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserInternal))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUserInternal, cancelationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserExternal.Mail))
             .Returns(true)
             .Verifiable();

            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await internalRegisterExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el caso cuando el usuario no se logra guardar en la bd
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(SaveNullException))]
        public async Task TestInternalRegisterUserExternal_SaveNullException()
        {

            var request = new InternalRegisterExternalCommand
            {
                UserInternalClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new InternalRegisterExternalCommand.NewUser
                {
                    Name = "Jose",
                    Lastname = "Avila",
                    Phone = "313456789",
                    Mail = "juan@gmail.com",
                    Password = "sdfsdf32",
                    Address = "Diagonal 4-34",
                }
            };

            var password = "julian10";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var idUserInternal = Guid.NewGuid();
            var idUserExternal = Guid.NewGuid();

            User userInternal = (User)factory.BuildUser(name, lastName, phone, mail, password, address, TipoUsuarioEnum.interno, token, idUserInternal);

            User userExternal = (User)factory.BuildUser("Jose", "Avila", "313456789", "juan@gmail.com", "sdfsdf32", "Diagonal 4-34", TipoUsuarioEnum.externo, "fsdfsdfs423", idUserExternal);

            User userExternalAux = null;

            InternalRegisterExternalEvent internalRegisterExternalEvent = new InternalRegisterExternalEvent()
            {
                Id = idUserExternal.ToString(),
                Name = "Jose",
                Lastname = "Avila",
                Mail = "juan@gmail.com",
                Token = "fsdfsdfs423",
                TipoUsuario = (int)TipoUsuarioEnum.externo
            };

            InternalRegisterExternalDTO internalRegisterExternalDTO = new InternalRegisterExternalDTO
            {
                Name = "Jose",
                Lastname = "Avila",
                Phone = "313456789",
                Mail = "juan@gmail.com",
                Password = "sdfsdf32",
                Address = "Diagonal 4-34",
                Token = "fsdfsdfs423"
            };

            var mailUserInternal = "cristhiancubides10@gmail.com";


            userSecurity.Setup(x => x.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL))
                .Returns(mailUserInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserInternal))
              .Returns(true)
              .Verifiable();

            userRepository.Setup(x => x.Get<User>(x => x.Mail == mailUserInternal, cancelationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserExternal.Mail))
              .Returns(false)
              .Verifiable();

            utilities.Setup(x => x.CreateId())
                .Returns(idUserExternal)
                .Verifiable();

            userSecurity.Setup(x => x.CreateToken(idUserExternal, request.NewUserExternal.Name, request.NewUserExternal.Mail, TipoUsuarioEnum.externo.ToString()))
                .Returns(token)
                .Verifiable();

            userFactory.Setup(x => x.BuildUser(userExternal.Name, userExternal.Lastname, userExternal.Phone, userExternal.Mail,
                userExternal.Password, userExternal.Address, userExternal.TipoUsuario, token, idUserExternal))
                .Returns(userExternal)
                .Verifiable();

            userRepository.Setup(x => x.Save<User>(userExternal, cancelationToken))
                .ReturnsAsync(userExternalAux)
                .Verifiable();

            var internalRegisterExternalHandler = new InternalRegisterExternalHandler(userRepository.Object, userSecurity.Object,
                userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await internalRegisterExternalHandler.Handle(request, cancelationToken);

        }

    }
}
