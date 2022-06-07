using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommandLoginUser
{
    public class LoginUserCommand : IRequest<LoginUserDTO>
    {
        public string Mail { get; set; }
        public string Password { get; set; }

    }
}
