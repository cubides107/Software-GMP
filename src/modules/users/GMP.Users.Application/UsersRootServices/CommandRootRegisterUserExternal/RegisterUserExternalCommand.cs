using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandRootRegisterUserExternal
{
    public class RegisterUserExternalCommand : IRequest<RegisterUserExternalDTO>
    {
        /// <summary>
        /// Claims del token del usuario Root
        /// </summary>
        public List<Claim> RootClaims { get; set; }

        /// <summary>
        /// Atributo de la clase usuario Externo
        /// </summary>
        public UserExternal NewUserExternal { get; set; }

        /// <summary>
        /// Nuevo Usuario Externo que crea el Usuario Root
        /// </summary>
        public class UserExternal
        {
            public string Name { get; set; }

            public string Lastname { get; set; }

            public string Phone { get; set; }

            public string Mail { get; set; }

            public string Password { get; set; }

            public string Address { get; set; }
        }

    }
}

