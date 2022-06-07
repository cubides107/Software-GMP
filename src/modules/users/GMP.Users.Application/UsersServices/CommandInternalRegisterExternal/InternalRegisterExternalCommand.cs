using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommandInternalRegisterExternal
{
    public class InternalRegisterExternalCommand : IRequest<InternalRegisterExternalDTO>
    {
        /// <summary>
        /// claims del usuario interno
        /// </summary>
        public List<Claim> UserInternalClaims { get; set; }

        /// <summary>
        /// datos del nuevo usuario Externo
        /// </summary>
        public NewUser NewUserExternal { get; set; }

        /// <summary>
        /// clase del nuevo usuario externo
        /// </summary>
        public class NewUser
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
