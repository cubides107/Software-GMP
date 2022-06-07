using MediatR;
using System.Collections.Generic;
using System.Security.Claims;

namespace GMP.Users.Application.UsersRootServices.CommandRestorePasswordUser
{
    public class RestorePasswordUserCommand : IRequest<RestorePasswordUserDTO>
    {
        /// <summary>
        /// Claims del usuario que realiza la peticion
        /// </summary>
        public List<Claim> RootClaims { get; set; }

        /// <summary>
        /// Mail del usuario del cual se va a restablecer la contraseña
        /// </summary>
        public string MailUserRestorePassword { get; set; }

        /// <summary>
        /// Nueva Contraseña
        /// </summary>
        public string NewPassword { get; set; }
    }
}
