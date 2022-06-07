using GMP.Apis.Web.Exceptions;
using GMP.Apis.Web.Filters;
using GMP.Users.Application.UsersServices.CommadEditUser;
using GMP.Users.Application.UsersServices.CommandInternalRegisterExternal;
using GMP.Users.Application.UsersServices.CommandLoginUser;
using GMP.Users.Application.UsersServices.CommandLogOut;
using GMP.Users.Application.UsersServices.QueryUser;
using GMP.Users.Application.UsersServices.QueryUsers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GMP.Apis.Web.UsersControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// atributo que representa el mediador entre la api y el cu
        /// </summary>
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api para el caso de que que el usuario interno registra un usuario externo
        /// </summary>
        /// <param name="internalRegisterExternalCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UsuarioInternoRegistraExterno")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PutInternalRegisterExternal(InternalRegisterExternalCommand internalRegisterExternalCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            //obtener los claims del usuario segun el token
            internalRegisterExternalCommand.UserInternalClaims = User.Claims.ToList();

            var dto = await mediator.Send(internalRegisterExternalCommand);

            return Ok(dto);
        }

        /// <summary>
        /// Api para logear usuario 
        /// </summary>
        /// <param name="loginUserCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginUser")]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PostLoginUser(LoginUserCommand loginUserCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            var dto = await mediator.Send(loginUserCommand);

            return Ok(dto);
        }

        /// <summary>
        /// Api para obtener los usuarios
        /// </summary>
        /// <param name="queryUsersCommand"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> GetUsers(string filterName, int page, int pageSize)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            var queryUsersCommand = new UsersByNameQuery
            {
                FilterName = filterName,
                Page = page,
                PageSize = pageSize,
                ClaimsUser = User.Claims.ToList()
            };

            var dto = await mediator.Send(queryUsersCommand);
            return Ok(dto);
        }

        /// <summary>
        /// Api para cerrar sesión de usuario
        /// </summary>
        /// <param name="logOutUserCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LogOut")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PostLogOut(LogOutUserCommand logOutUserCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            logOutUserCommand.UserClaims = User.Claims.ToList();

            var dto = await mediator.Send(logOutUserCommand);
            return Ok(dto);
        }

        /// <summary>
        /// Api para obtener datos de usuario
        /// </summary>
        /// <param name="queryUser"></param>
        /// <returns></returns>
        /// <exception cref="ModelStateIsInvalid"></exception>
        [HttpGet]
        [Route("QueryUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> GetUser()
        {
            if(!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            QueryUser queryUser = new ();

            queryUser.UserClaims = User.Claims.ToList();
            var dto = await mediator.Send(queryUser);
            return Ok(dto);
        }

        /// <summary>
        /// Api para editar un usuario
        /// </summary>
        /// <param name="editUserCommand"></param>
        /// <returns></returns>
        /// <exception cref="ModelStateIsInvalid"></exception>
        [HttpPost]
        [Route("EditUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PostEditUser(EditUserCommand editUserCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            editUserCommand.UserClaims = User.Claims.ToList();

            var dto = await mediator.Send(editUserCommand);
            return Ok(dto);
        }
    }
}
