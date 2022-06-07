using GMP.Apis.Web.Exceptions;
using GMP.Apis.Web.Filters;
using GMP.Users.Application.UsersRootServices.CommandAccessRoot;
using GMP.Users.Application.UsersRootServices.CommandRestorePasswordUser;
using GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal;
using GMP.Users.Application.UsersRootServices.CommandRootRegisterUserExternal;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GMP.Apis.Web.UsersControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersRootController : ControllerBase
    {
        private readonly IMediator mediator;
        public UsersRootController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api para el acceso del usuario root
        /// </summary>
        /// <param name="accesRootCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AccesRoot")]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PostAccesRoot(AccessRootCommand accesRootCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            var dto = await mediator.Send(accesRootCommand);

            return Ok(dto);
        }

        /// <summary>
        /// Api para el registro de un usuario interno por medio del usuario root
        /// </summary>
        /// <param name="registerUserInternalCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RootRegisterUserInternal")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PutRootRegisterUserInternal(RegistersUserInternalCommand registerUserInternalCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            //Obtener claims del usuario Root
            registerUserInternalCommand.RootClaims = User.Claims.ToList();

            var dto = await mediator.Send(registerUserInternalCommand);

            return Ok(dto);
        }

        /// <summary>
        /// Api para el registro de un usuario externo por medio del usuario root
        /// </summary>
        /// <param name="registerUserExternalCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RootRegisterUserExternal")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PutRootRegisterUserExternal(RegisterUserExternalCommand registerUserExternalCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            //Obtener claims del usuario Root
            registerUserExternalCommand.RootClaims = User.Claims.ToList();

            var dto = await mediator.Send(registerUserExternalCommand);

            return Ok(dto);
        }

        /// <summary>
        /// Api para restablecer la contraseña de los usuarios
        /// </summary>
        /// <param name="restorePasswordUserCommand"></param>
        /// <returns></returns>
        /// <exception cref="ModelStateIsInvalid"></exception>
        [HttpPost]
        [Route("RestorePasswordUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PostRestorePasswordUser(RestorePasswordUserCommand restorePasswordUserCommand)
        {
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            //Obtener claims del usuario Root
            restorePasswordUserCommand.RootClaims = User.Claims.ToList();

            var dto = await mediator.Send(restorePasswordUserCommand);

            return Ok(dto);
        }
    }
}
