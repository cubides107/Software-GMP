using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommandLogOut
{
    public class LogOutUserCommand : IRequest<LogOutUserDTO>
    {
        public List<Claim> UserClaims { get; set; }
    }
}
