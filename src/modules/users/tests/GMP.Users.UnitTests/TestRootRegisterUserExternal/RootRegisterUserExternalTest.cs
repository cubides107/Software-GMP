using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersRootServices.CommandRootRegisterUserExternal;
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

namespace GMP.Users.UnitTests.TestRootRegisterUserExternal
{
    [TestClass]
    public class RootRegisterUserExternalTest
    {
        private readonly Mock<IUsersFactories> userFactory;
        private readonly Mock<IUsersRepositories> userRepository;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IEventPublisher> eventPublisher;

        private readonly UsersFactories factory;

        public RootRegisterUserExternalTest()
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
        public async Task TestRootUserExternalHandler()
        {
            var request = new RegisterUserExternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new RegisterUserExternalCommand.UserExternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "julian10",
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

            RegisterUserExternalDTO userAux = new RegisterUserExternalDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhiancubides10@gmail.com",
                Password = "4werwer4dfgdfg",
                Address = "Carrera 1 este # 33-34"

            };

            RootRegisterUserExternalEvent userEvent = new RootRegisterUserExternalEvent
            {
                Name = "Julian",
                Lastname = "Cubides",
                Mail = "cristhiancubides10@gmail.com",
                Token = token,
                TipoUsuario = 1
            };

            User user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.externo, token, id);

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

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserExternal.Password))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserExternal.Mail))
               .Returns(false)
               .Verifiable();

            utilities.Setup(x => x.CreateId())
               .Returns(id)
               .Verifiable();

            userFactory.Setup(x => x.BuildUser(request.NewUserExternal.Name, request.NewUserExternal.Lastname, request.NewUserExternal.Phone, request.NewUserExternal.Mail,
                encryptPassword, request.NewUserExternal.Address, TipoUsuarioEnum.externo, null, id))
              .Returns(user)
              .Verifiable();

            userRepository.Setup(x => x.Save(user, cancelationToken))
              .ReturnsAsync(user)
              .Verifiable();

            mapping.Setup(x => x.Map<User, RegisterUserExternalDTO>(user))
                .Returns(userAux)
                .Verifiable();

            mapping.Setup(x => x.Map<User, RootRegisterUserExternalEvent>(user))
                .Returns(userEvent)
                .Verifiable();

            eventPublisher.Setup(x => x.PublishEventAsync(userEvent))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var registerUserExternalHandler = new RegisterUserExternalHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            RegisterUserExternalDTO registerUserExternalAux = await registerUserExternalHandler.Handle(request, cancelationToken);

            Assert.AreEqual(userAux, registerUserExternalAux);
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando la peticion esta nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestRootRegisterUserExternal_CommandRequestNullException()
        {
            var registerUserExternalHandler = new RegisterUserExternalHandler(userRepository.Object, userSecurity.Object,
               userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando el usuario no es root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestRootRegisterUserExternal_NotRootException_Mail()
        {
            var request = new RegisterUserExternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new RegisterUserExternalCommand.UserExternal
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
                   .Returns(request.NewUserExternal.Mail)
                   .Verifiable();


            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            var registerUserExternalHandler = new RegisterUserExternalHandler(userRepository.Object, userSecurity.Object,
             userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando la contraseña no corresponde a un usuario root
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(NotRootException))]
        public async Task TestRootRegisterUserExternal_NotRootException_Password()
        {
            var request = new RegisterUserExternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new RegisterUserExternalCommand.UserExternal
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
            var password = "julian10";

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT))
                   .Returns(request.NewUserExternal.Mail)
                   .Verifiable();


            userSecurity.Setup(x => x.GetClaim(request.RootClaims, IUserSecurity.MAIL))
                .Returns(mailUserRoot)
                .Verifiable();

            utilities.Setup(x => x.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT))
                .Returns(password)
                .Verifiable();

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserExternal.Password))
                .Returns(password)
                .Verifiable();

            var registerUserExternalHandler = new RegisterUserExternalHandler(userRepository.Object, userSecurity.Object,
             userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, new CancellationToken());
        }

        /// <summary>
        /// Se comprueba el lanzamiento de la excepción cuando no se guardo el usuario en la bd
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(SaveNullException))]
        public async Task TestRootUserExternalHandler_SaveNullException()
        {
            var request = new RegisterUserExternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new RegisterUserExternalCommand.UserExternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "julian10",
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

            RegisterUserExternalDTO userAux = new RegisterUserExternalDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhiancubides10@gmail.com",
                Password = "4werwer4dfgdfg",
                Address = "Carrera 1 este # 33-34"

            };

            RootRegisterUserExternalEvent userEvent = new RootRegisterUserExternalEvent
            {
                Name = "Julian",
                Lastname = "Cubides",
                Mail = "cristhiancubides10@gmail.com",
                Token = token,
                TipoUsuario = 1
            };

            User user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.externo, token, id);

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

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserExternal.Password))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserExternal.Mail))
               .Returns(false)
               .Verifiable();

            userSecurity.Setup(x => x.CreateToken(id, request.NewUserExternal.Name, request.NewUserExternal.Mail, TipoUsuarioEnum.externo.ToString()))
                .Returns(token)
                .Verifiable();

            utilities.Setup(x => x.CreateId())
               .Returns(id)
               .Verifiable();

            userFactory.Setup(x => x.BuildUser(request.NewUserExternal.Name, request.NewUserExternal.Lastname, request.NewUserExternal.Phone, request.NewUserExternal.Mail,
                encryptPassword, request.NewUserExternal.Address, TipoUsuarioEnum.externo, token, id))
              .Returns(user)
              .Verifiable();

            userRepository.Setup(x => x.Save(user, cancelationToken));

            mapping.Setup(x => x.Map<User, RegisterUserExternalDTO>(user))
                .Returns(userAux)
                .Verifiable();

            var registerUserExternalHandler = new RegisterUserExternalHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, cancelationToken);
        }

        /// <summary>
        /// caso si el usuario que tratamos de registrar ya existe
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserExistsException))]
        public async Task TestRootUserExternalHandler_UserExistsException()
        {
            var request = new RegisterUserExternalCommand
            {
                RootClaims = new List<System.Security.Claims.Claim>
                {

                },

                NewUserExternal = new RegisterUserExternalCommand.UserExternal
                {
                    Name = "Julian",
                    Lastname = "Cubides",
                    Phone = "3138989123",
                    Mail = "cristhiancubides10@gmail.com",
                    Password = "julian10",
                    Address = "Carrera 1 este # 33-34"
                }
            };

            var name = "Julian";
            var passwordUserRoot = "sdfsdfsdfsdf324";
            var cancelationToken = new CancellationToken();
            var lastName = "Cubides";
            var phone = "3138989123";
            var mail = "cristhiancubides10@gmail.com";
            var address = "Carrera 1 Este #33-34";
            string token = "jsusjsuj8983j45.";
            var mailUserRoot = "cristhiancubides10@gmail.com";
            var encryptPassword = "4werwer4dfgdfg";
            var id = Guid.NewGuid();

            RegisterUserExternalDTO userAux = new RegisterUserExternalDTO
            {
                Name = "Julian",
                Lastname = "Cubides",
                Phone = "3138989123",
                Mail = "cristhiancubides10@gmail.com",
                Password = "4werwer4dfgdfg",
                Address = "Carrera 1 este # 33-34"

            };

            RootRegisterUserExternalEvent userEvent = new RootRegisterUserExternalEvent
            {
                Name = "Julian",
                Lastname = "Cubides",
                Mail = "cristhiancubides10@gmail.com",
                Token = token,
                TipoUsuario = 1
            };

            User user = (User)factory.BuildUser(name, lastName, phone, mail, encryptPassword, address, TipoUsuarioEnum.externo, token, id);

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

            userSecurity.Setup(x => x.EncryptPassword(request.NewUserExternal.Password))
                .Returns(encryptPassword)
                .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == mailUserRoot && x.Password == encryptPassword))
               .Returns(true)
               .Verifiable();

            userRepository.Setup(x => x.Exists<User>(x => x.Mail == request.NewUserExternal.Mail))
               .Returns(true)
               .Verifiable();

            var registerUserExternalHandler = new RegisterUserExternalHandler(userRepository.Object, userSecurity.Object,
              userFactory.Object, utilities.Object, mapping.Object, eventPublisher.Object);

            await registerUserExternalHandler.Handle(request, cancelationToken);

        }
    }
}
