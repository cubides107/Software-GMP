using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersServices.QueryUsers;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestQueryUsers
{
    [TestClass]
    public class QueryUsersTest
    {
        private readonly Mock<IUsersRepositories> usersRepositories;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly UsersFactories factory;

        public QueryUsersTest()
        {
            usersRepositories = new Mock<IUsersRepositories>();
            mapping = new Mock<IAutoMapping>();
            userSecurity = new Mock<IUserSecurity>();
            factory = new UsersFactories();
        }

        /// <summary>
        /// Flujo normal caso de uso para obtener lista de usuarios
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestQueryUsers()
        {
            var request = new UsersByNameQuery
            {
                ClaimsUser = new System.Collections.Generic.List<System.Security.Claims.Claim> { },
                FilterName = "Jose",
                Page = 0,
                PageSize = 1
            };

            var mailUser = "julian@gmail.com";
            var cantidadRegistros = 1;
            var cancellationToken = new CancellationToken();
            User userInternal = (User)factory.BuildUser("Jose", "Avila", "314567890", "jose@gmail.com", "fsdfsdf34324", "Diagonal 6#3-56", TipoUsuarioEnum.interno, "fghfghrtyr", null);
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo, "34534ffgdsfg", null);

            var userAuXDTO = new UsersByNameDTO.UsersDTO
            {
                Name = "Juan",
                Lastname = "Avila",
                Phone = "314347890",
                Mail = "juan@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.externo
            };

            List<User> listUsers = new List<User>();
            listUsers.Add(userAux);

            var listUsersDTO = new List<UsersByNameDTO.UsersDTO>();
            listUsersDTO.Add(userAuXDTO);


            userSecurity.Setup(x => x.GetClaim(request.ClaimsUser, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            usersRepositories.Setup(x => x.GetAll<User>(x => x.Name, request.Page, request.PageSize, x => x.Name == request.FilterName, cancellationToken))
                .ReturnsAsync(listUsers)
                .Verifiable();

            mapping.Setup(x => x.Map<List<User>, List<UsersByNameDTO.UsersDTO>>(listUsers))
               .Returns(listUsersDTO)
               .Verifiable();

            usersRepositories.Setup(x => x.Count<User>(x => x.Name == request.FilterName, cancellationToken))
                .ReturnsAsync(cantidadRegistros)
                .Verifiable();

            var queryUsers = new UsersByNameHandler(usersRepositories.Object, mapping.Object, userSecurity.Object);

            UsersByNameDTO usersDTO = await queryUsers.Handle(request, cancellationToken);

            Assert.AreEqual(listUsersDTO, usersDTO.ListUsers);
        }

        /// <summary>
        /// Lanzamiento de Excepción cuando la petición es nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestQueryUsers_CommandRequestNullException()
        {
            var queryUsers = new UsersByNameHandler(usersRepositories.Object, mapping.Object, userSecurity.Object);

            await queryUsers.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Lanzamiento de Excepción cuando el usuario no esta registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestQueryUsers_UserNotRegisterException()
        {
            var request = new UsersByNameQuery
            {
                ClaimsUser = new System.Collections.Generic.List<System.Security.Claims.Claim> { },
                FilterName = "Jose",
                Page = 0,
                PageSize = 1
            };

            var mailUser = "julian@gmail.com";
            var cancellationToken = new CancellationToken();
            User userInternal = (User)factory.BuildUser("Jose", "Avila", "314567890", "jose@gmail.com", "fsdfsdf34324", "Diagonal 6#3-56", TipoUsuarioEnum.interno, "fghfghrtyr", null);
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo, "34534ffgdsfg", null);

            var userAuXDTO = new UsersByNameDTO.UsersDTO
            {
                Name = "Juan",
                Lastname = "Avila",
                Phone = "314347890",
                Mail = "juan@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.externo
            };

            List<User> listUsers = new List<User>();
            listUsers.Add(userAux);

            var listUsersDTO = new List<UsersByNameDTO.UsersDTO>();
            listUsersDTO.Add(userAuXDTO);


            userSecurity.Setup(x => x.GetClaim(request.ClaimsUser, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(false)
              .Verifiable();

            var queryUsers = new UsersByNameHandler(usersRepositories.Object, mapping.Object, userSecurity.Object);

            UsersByNameDTO usersDTO = await queryUsers.Handle(request, cancellationToken);
        }

        /// <summary>
        /// Lanzamiento de Excepción cuando el usuario no se puede obtener
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestQueryUsers_EntityNotRecoveredException()
        {
            var request = new UsersByNameQuery
            {
                ClaimsUser = new System.Collections.Generic.List<System.Security.Claims.Claim> { },
                FilterName = "Jose",
                Page = 0,
                PageSize = 1
            };

            var mailUser = "julian@gmail.com";
            var cancellationToken = new CancellationToken();
            User userInternal = null;
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo, "34534ffgdsfg", null);

            var userAuXDTO = new UsersByNameDTO.UsersDTO
            {
                Name = "Juan",
                Lastname = "Avila",
                Phone = "314347890",
                Mail = "juan@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.externo
            };

            List<User> listUsers = new List<User>();
            listUsers.Add(userAux);

            var listUsersDTO = new List<UsersByNameDTO.UsersDTO>();
            listUsersDTO.Add(userAuXDTO);


            userSecurity.Setup(x => x.GetClaim(request.ClaimsUser, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            var queryUsers = new UsersByNameHandler(usersRepositories.Object, mapping.Object, userSecurity.Object);

            await queryUsers.Handle(request, cancellationToken);
        }

        /// <summary>
        /// Lanzamiento de Excepción cuando el usuario es un usuario externo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(PermissionQueryUserException))]
        public async Task TestQueryUsers_PermissionQueryUserException()
        {
            var request = new UsersByNameQuery
            {
                ClaimsUser = new System.Collections.Generic.List<System.Security.Claims.Claim> { },
                FilterName = "Jose",
                Page = 0,
                PageSize = 1
            };

            var mailUser = "julian@gmail.com";
            var cancellationToken = new CancellationToken();
            User userInternal = (User)factory.BuildUser("Jose", "Avila", "314567890", "jose@gmail.com", "fsdfsdf34324", "Diagonal 6#3-56", TipoUsuarioEnum.externo, "fghfghrtyr", null);
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo, "34534ffgdsfg", null);

            var userAuXDTO = new UsersByNameDTO.UsersDTO
            {
                Name = "Juan",
                Lastname = "Avila",
                Phone = "314347890",
                Mail = "juan@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.externo
            };

            List<User> listUsers = new List<User>();
            listUsers.Add(userAux);

            var listUsersDTO = new List<UsersByNameDTO.UsersDTO>();
            listUsersDTO.Add(userAuXDTO);


            userSecurity.Setup(x => x.GetClaim(request.ClaimsUser, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            var queryUsers = new UsersByNameHandler(usersRepositories.Object, mapping.Object, userSecurity.Object);

            await queryUsers.Handle(request, cancellationToken);
        }

        /// <summary>
        /// Lanzamiento de Excepción cuando la lista obtenida es nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(ListUsersException))]
        public async Task TestQueryUsers_ListUsersException_ListNull()
        {
            var request = new UsersByNameQuery
            {
                ClaimsUser = new System.Collections.Generic.List<System.Security.Claims.Claim> { },
                FilterName = "Jose",
                Page = 0,
                PageSize = 1
            };

            var mailUser = "julian@gmail.com";
            var cancellationToken = new CancellationToken();
            User userInternal = (User)factory.BuildUser("Jose", "Avila", "314567890", "jose@gmail.com", "fsdfsdf34324", "Diagonal 6#3-56", TipoUsuarioEnum.interno, "fghfghrtyr", null);

            var userAuXDTO = new UsersByNameDTO.UsersDTO
            {
                Name = "Juan",
                Lastname = "Avila",
                Phone = "314347890",
                Mail = "juan@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.externo
            };

            List<User> listUsers = null;

            userSecurity.Setup(x => x.GetClaim(request.ClaimsUser, IUserSecurity.MAIL))
                .Returns(mailUser)
                .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Mail == mailUser))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Mail == mailUser, cancellationToken))
                .ReturnsAsync(userInternal)
                .Verifiable();

            usersRepositories.Setup(x => x.GetAll<User>(x => x.Name, request.Page, request.PageSize, x => x.Name == request.FilterName, cancellationToken))
                .ReturnsAsync(listUsers)
                .Verifiable();

            var queryUsers = new UsersByNameHandler(usersRepositories.Object, mapping.Object, userSecurity.Object);

            await queryUsers.Handle(request, cancellationToken);

        }

    }

}
