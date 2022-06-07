using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandAccessRoot
{
    public class AccessRootCommand : IRequest<AccessRootDTO>
    {
        public string Mail { get; set; }

        public string Password { get; set; }
    }
}
