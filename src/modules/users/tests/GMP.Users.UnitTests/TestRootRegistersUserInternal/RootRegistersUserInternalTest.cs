using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestRootRegistersUserInternal
{
    [TestClass]
    public class RootRegistersUserInternalTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;

        private readonly UsersFactories factory;

        public RootRegistersUserInternalTest()
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
        /// Se compruba el flujo basico del caso de uso donde el usuario root registra un usuario externo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestRootUserInternalHandler()
        {

            var request = new RegistersUserInternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserInternal = new RegistersUserInternalCommand.UserInternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "4werwer4dfgdfg",
                    Address = "Carrera 1 este # 33-34"
                }
            };
          
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var mailUserRoot = "cristhiancubides10@gmail.com";
            var encryptPassword = "4werwer4dfgdfg";
            var id = Guid.NewGuid();

            var userAux = new RegistersUserInternalDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhiancubides10@gmail.com",
                Password = "julian10",
                Address = "Carrera 1 este # 33-34"
            };

            RootRegisterUserInternalEvent userEvent = new RootRegisterUserInternalEvent
            {
                Name = "Julian",
                Lastname = "Cubides",
                Mail = "cristhiancubides10@gmail.com",
                Token = token,
                TipoUsuario = 1
            };


            User user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, token, id);

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(mailUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(encryptPassword)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserInternal.Password))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserInternal.Mail))
               .Returns(false)
               .Verifiable();

            utilities.Setup(x => x.CreateId())
               .Returns(id)
               .Verifiable();

            userFactory.Setup(x => x.BuildUser(
                request.NewUserInternal.Name,
                request.NewUserInternal.Lastname,
                request.NewUserInternal.Phone,
                request.NewUserInternal.Mail,
                request.NewUserInternal.Password,
                request.NewUserInternal.Address,
                TipoUsuarioEnum.interno,
                null, id))
              .Returns(user)
              .Verifiable();

            userRepository.Setup(x => x.Save(user, cancelationToken))
              .ReturnsAsync(user)
              .Verifiable();

            mapping.Setup(x => x.Map<User, RegistersUserInternalDTO>(user))
                .Returns(userAux)
                .Verifiable();

            mapping.Setup(x => x.Map<User, RootRegisterUserInternalEvent>(user))
                 .Returns(userEvent)
                 .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(userEvent))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var registersUserInternalHandler = new RegistersUserInternalHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            RegistersUserInternalDTO registerUserExternalAux = await registersUserInternalHandler.Handle(request, cancelationToken);

            Assert.AreEqual(userAux, registerUserExternalAux);
        }

        /// <summary>
        /// caso cuando la peticion es nula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestRootUserInternalHandler_CommandRequestNullException()
        {
            var cancellationToken = new CancellationToken();

            var registersUserInternalHandler = new RegistersUserInternalHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            RegistersUserInternalDTO registerUserExternalAux = await registersUserInternalHandler.Handle(null, cancellationToken);
        }

        /// <summary>
        /// caso cuando el correo del usuario root no corresponde en las variables de entorno
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestRootUserInternalHandler_NotRootException_Mail()
        {
            var request = new RegistersUserInternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserInternal = new RegistersUserInternalCommand.UserInternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "julian10",
                    Address = "Carrera 1 este # 33-34"
                }
            };
            var mailUserRoot = "cristhian10@gmail.com";

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                   .Returns(request.NewUserInternal.Mail)
                   .Verifiable();


            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            var registerUserExternalHandler = new RegistersUserInternalHandler(userRepository.Object, userSecurity.Object,
             userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// caso cuando la contraseña del usuario root y el mail no corresponden en la db
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestRootUserInternalHandler_NotRootException_Password()
        {
            var request = new RegistersUserInternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserInternal = new RegistersUserInternalCommand.UserInternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "julian10",
                    Address = "Carrera 1 este # 33-34"
                }
            };
            var mailUserRoot = "cristhiancubides10@gmail.com";
            var password = "secar123";
            var encryptPassword = "dhhshd2323";

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                   .Returns(request.NewUserInternal.Mail)
                   .Verifiable();


            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(password)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserInternal.Password))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(false)
               .Verifiable();


            var registerUserExternalHandler = new RegistersUserInternalHandler(userRepository.Object, userSecurity.Object,
             userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// cuando el usuario que se va a crear ya existe 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserExistsException))]
        public async Task TestRootUserInternalHandler_UserExistsException()
        {
            var request = new RegistersUserInternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserInternal = new RegistersUserInternalCommand.UserInternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "4werwer4dfgdfg",
                    Address = "Carrera 1 este # 33-34"
                }
            };
          
            var passwordUserRoot = "sdfsdfsdfsdf324";
            var name = "Julian";
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var mailUserRoot = "cristhiancubides10@gmail.com";
            var encryptPassword = "4werwer4dfgdfg";
            var id = Guid.NewGuid();

            var userAux = new RegistersUserInternalDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhiancubides10@gmail.com",
                Password = "julian10",
                Address = "Carrera 1 este # 33-34"
            };

            RootRegisterUserInternalEvent userEvent = new RootRegisterUserInternalEvent
            {
                Name = "Julian",
                Lastname = "Cubides",
                Mail = "cristhiancubides10@gmail.com",
                Token = token,
                TipoUsuario = 1
            };


            User user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, token, id);

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(mailUserRoot)
                .Verifiable();


            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(passwordUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(passwordUserRoot))
                .Returns(encryptPassword)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserInternal.Password))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserInternal.Mail))
               .Returns(true)
               .Verifiable();


            var registerUserExternalHandler = new RegistersUserInternalHandler(userRepository.Object, userSecurity.Object,
             userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// caso cuando al guardar regresa nulo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(SaveNullException))]
        public async Task TestRootUserInternalHandler_SaveNullException()
        {

            var request = new RegistersUserInternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserInternal = new RegistersUserInternalCommand.UserInternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "4werwer4dfgdfg",
                    Address = "Carrera 1 este # 33-34"
                }
            };
          
            var passwordUserRoot = "sdfsdfsdfsdf324";
            var name = "Julian";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var mailUserRoot = "cristhiancubides10@gmail.com";
            var encryptPassword = "4werwer4dfgdfg";
            var id = Guid.NewGuid();

            var userAux = new RegistersUserInternalDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhiancubides10@gmail.com",
                Password = "julian10",
                Address = "Carrera 1 este # 33-34"
            };

            RootRegisterUserInternalEvent userEvent = new RootRegisterUserInternalEvent
            {
                Name = "Julian",
                Lastname = "Cubides",
                Mail = "cristhiancubides10@gmail.com",
                Token = token,
                TipoUsuario = 1
            };


            User user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.interno, token, id);

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                .Returns(mailUserRoot)
                .Verifiable();


            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(passwordUserRoot)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(passwordUserRoot))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserInternal.Mail))
               .Returns(false)
               .Verifiable();

            userSecurity.Setup(x => x.CreateToken(id, request.NewUserInternal.Name, request.NewUserInternal.Mail, TipoUsuarioEnum.interno.ToString()))
                .Returns(token)
                .Verifiable();

            utilities.Setup(x => x.CreateId())
               .Returns(id)
               .Verifiable();

            userFactory.Setup(x => x.BuildUser(
                request.NewUserInternal.Name,
                request.NewUserInternal.Lastname,
                request.NewUserInternal.Phone,
                request.NewUserInternal.Mail,
                request.NewUserInternal.Password,
                request.NewUserInternal.Address,
                TipoUsuarioEnum.interno,
                token, id))
              .Returns(user)
              .Verifiable();

            userRepository.Setup(x => x.Save(user, cancelationToken));

            var registersUserInternalHandler = new RegistersUserInternalHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            RegistersUserInternalDTO registerUserExternalAux = await registersUserInternalHandler.Handle(request, cancelationToken);

        }
    }
}
