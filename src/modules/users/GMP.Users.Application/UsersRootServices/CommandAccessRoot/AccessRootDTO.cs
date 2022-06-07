using GMP.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandAccessRoot
{
    public class AccessRootDTO : DTO<AccessRootDTO>
    {
        public string Token { get; set; }
    }
}
