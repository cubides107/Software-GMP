using GMP.Application;
using GMP.Users.Domain.Entities;
using System.Collections.Generic;

namespace GMP.Users.Application.UsersServices.QueryUsers
{
    public class UsersByNameDTO : DTO<UsersByNameDTO>
    {
        /// <summary>
        /// lista de usuarios segun consulta
        /// </summary>
        public List<UsersDTO> ListUsers { get; set; }

        /// <summary>
        /// numero que representa la cantidad de registros
        /// nos permite saber cuantas paginas tendra la consulta
        /// </summary>
        public int CountUsers { get; set; }

        /// <summary>
        /// clase anidada que representa el usuarios
        /// </summary>
        public class UsersDTO
        {
            public string Name { get; set; }

            public string Lastname { get; set; }

            public string Phone { get; set; }

            public string Mail { get; set; }

            public string Address { get; set; }

            public TipoUsuarioEnum TipoUsuario { get; set; }
        }
    }
}