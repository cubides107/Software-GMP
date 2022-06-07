using GMP.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommandLoginUser
{
   public class LoginUserDTO : DTO<LoginUserDTO>
    {
        public string Token { get; set; }
   }
    
}
