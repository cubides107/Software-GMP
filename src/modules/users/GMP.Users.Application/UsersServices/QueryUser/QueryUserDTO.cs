using GMP.Application;
using GMP.Users.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.QueryUser
{
    public class QueryUserDTO: DTO<QueryUserDTO>
    {
            public string Name { get; set; }

            public string LastName { get; set; }

            public string Phone { get; set; }

            public string Mail { get; set; }

            public string Address { get; set; }

            public TipoUsuarioEnum TipoUsuario { get; set; }
    }
}
