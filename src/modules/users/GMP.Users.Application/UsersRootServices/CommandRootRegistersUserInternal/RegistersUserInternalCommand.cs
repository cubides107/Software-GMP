using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal
{
    public class RegistersUserInternalCommand : IRequest<RegistersUserInternalDTO>
    {
        /// <summary>
        /// claims del token del usuario root
        /// </summary>
        public List<Claim> RootClaims { get; set; }

        /// <summary>
        /// atirbuto de la clase nuevo usuario intenal
        /// </summary>
        public UserInternal NewUserInternal { get; set; }

        /// <summary>
        /// nuevo usuario interno que crea el root
        /// </summary>
        public class UserInternal
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
