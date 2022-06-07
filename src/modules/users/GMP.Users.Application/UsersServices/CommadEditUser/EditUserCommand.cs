using GMP.Users.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Security.Claims;

namespace GMP.Users.Application.UsersServices.CommadEditUser
{
    public class EditUserCommand : IRequest<EditUserDTO>
    {
        /// <summary>
        /// Claims del usuario que realiza la peticion
        /// </summary>
        public List<Claim> UserClaims { get; set; }    

        /// <summary>
        /// Id del usuario a editar
        /// </summary>
        public string UserIdToEdit { get; set; }

        /// <summary>
        /// Objeto con los campos del usuario a editar
        /// </summary>
        public DataUserEdit DataUserToEdit { get; set; }

        /// <summary>
        /// Clase con lo datos para editar el usuario
        /// </summary>
        public class DataUserEdit
        {
            public string Name { get; set; }

            public string LastName { get; set; }

            public string Phone { get; set; }

            public string Mail { get; set; }

            public string Address { get; set; }

            public TipoUsuarioEnum TipoUsuario {get;set;}
        }
    }
}
