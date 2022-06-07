using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Application.UsersServices.QueryUser;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.UnitTests.TestQueryUser
{
    [TestClass]
    public class QueryUserTest
    {
        private readonly Mock<IUserSecurity> userSecurity;
        private readonly Mock<IAutoMapping> mapping;
        private readonly Mock<IUsersRepositories> usersRepositories;
        private readonly UsersFactories factory;

        public QueryUserTest()
        {
            userSecurity = new Mock<IUserSecurity>();
            usersRepositories = new Mock<IUsersRepositories>();
            mapping = new Mock<IAutoMapping>();
            factory = new UsersFactories();
        }

        /// <summary>
        /// Flujo normal para caso de uso obtener usuario
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestQueryUser()
        {
            var request = new QueryUser
            {
                UserClaims = new System.Collections.Generic.List<System.Security.Claims.Claim> { }
            };

            var cancellationToken = new CancellationToken();
            var userId = "9735aa73-673a-4608-8992-6dfbb146242e";
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo, token:"34534ffgdsfg");
            var userDTO = new QueryUserDTO
            {
                Name = "Jose", 
                LastName = "Avila", 
                Phone = "314567890", 
                Mail = "jose@gmail.com", 
                Address = "Diagonal 6#3-56", 
                TipoUsuario = TipoUsuarioEnum.interno
            };

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.USERID))
               .Returns(userId)
               .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Id == userId))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Id == userId, cancellationToken))
               .ReturnsAsync(userAux)
               .Verifiable();

            mapping.Setup(x => x.Map<User, QueryUserDTO>(userAux))
               .Returns(userDTO)
               .Verifiable();

            var queryUser = new QueryUserHandler(userSecurity.Object,usersRepositories.Object, mapping.Object);

            QueryUserDTO queryUserDTO = await queryUser.Handle(request, cancellationToken);

            Assert.AreEqual(userDTO, queryUserDTO);
        }

        /// <summary>
        /// Lanzamiento de Excepción cuando la peticion esta nula
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(CommandRequestNullException))]
        public async Task TestQueryUser_RequestNullException()
        {
            var queryUser = new QueryUserHandler(userSecurity.Object, usersRepositories.Object, mapping.Object);

            await queryUser.Handle(null, new CancellationToken());
        }

        /// <summary>
        /// Lanzamiento de excepción cuando el usuario no esta registrado
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(UserNotRegisterException))]
        public async Task TestQueryUser_UserNotRegisterException()
        {
            var request = new QueryUser
            {
                UserClaims = new System.Collections.Generic.List<System.Security.Claims.Claim> { }
            };

            var cancellationToken = new CancellationToken();
            var userId = "9735aa73-673a-4608-8992-6dfbb146242e";
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo, "34534ffgdsfg", null);
            var userDTO = new QueryUserDTO
            {
                Name = "Jose",
                LastName = "Avila",
                Phone = "314567890",
                Mail = "jose@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.interno
            };

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.USERID))
               .Returns(userId)
               .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Id == userId))
              .Returns(false)
              .Verifiable();

            var queryUser = new QueryUserHandler(userSecurity.Object, usersRepositories.Object, mapping.Object);

            await queryUser.Handle(request, cancellationToken);

        }

        /// <summary>
        /// Lanzamiento de excepción cuando el usuario esta nulo
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(EntityNotRecoveredException))]
        public async Task TestQueryUser_EntityNotRecoveredException()
        {
            var request = new QueryUser
            {
                UserClaims = new System.Collections.Generic.List<System.Security.Claims.Claim> { }
            };

            var cancellationToken = new CancellationToken();
            var userId = "9735aa73-673a-4608-8992-6dfbb146242e";
            User userAux = null;
            var userDTO = new QueryUserDTO
            {
                Name = "Jose",
                LastName = "Avila",
                Phone = "314567890",
                Mail = "jose@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.interno
            };

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.USERID))
               .Returns(userId)
               .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Id == userId))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Id == userId, cancellationToken))
               .ReturnsAsync(userAux)
               .Verifiable();

            var queryUser = new QueryUserHandler(userSecurity.Object, usersRepositories.Object, mapping.Object);

            await queryUser.Handle(request, cancellationToken);
        }

        /// <summary>
        /// Lanzamiento de excepción cuando el token  esta nulo, es decir el usuario no ha iniciado sesión
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(TokenNullException))]
        public async Task TestQueryUser_EntityNotRecoveredException_TokenNull()
        {
            var request = new QueryUser
            {
                UserClaims = new System.Collections.Generic.List<System.Security.Claims.Claim> { }
            };

            var cancellationToken = new CancellationToken();
            var userId = "9735aa73-673a-4608-8992-6dfbb146242e";
            User userAux = (User)factory.BuildUser("Juan", "Avila", "314347890", "juan@gmail.com", "hgjfghjdfgh5", "Diagonal 6#3-56", TipoUsuarioEnum.externo);
            var userDTO = new QueryUserDTO
            {
                Name = "Jose",
                LastName = "Avila",
                Phone = "314567890",
                Mail = "jose@gmail.com",
                Address = "Diagonal 6#3-56",
                TipoUsuario = TipoUsuarioEnum.interno
            };

            userSecurity.Setup(x => x.GetClaim(request.UserClaims, IUserSecurity.USERID))
               .Returns(userId)
               .Verifiable();

            usersRepositories.Setup(x => x.Exists<User>(x => x.Id == userId))
              .Returns(true)
              .Verifiable();

            usersRepositories.Setup(x => x.Get<User>(x => x.Id == userId, cancellationToken))
               .ReturnsAsync(userAux)
               .Verifiable();

            var queryUser = new QueryUserHandler(userSecurity.Object, usersRepositories.Object, mapping.Object);

            await queryUser.Handle(request, cancellationToken);
        }
    }

}
