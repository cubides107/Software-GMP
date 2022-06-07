using GMP.Application;
using GMP.Users.Domain.Entities;

namespace GMP.Users.Application.UsersServices.CommadEditUser
{
    public class EditUserDTO : DTO<EditUserDTO>
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Phone { get; set; }

        public string Mail { get; set; }

        public string Address { get; set; }

        public TipoUsuarioEnum TipoUsuario { get; set; }

    }
}
