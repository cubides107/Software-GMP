using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Domain.Ports
{
    public interface IUserSecurity
    {
        public string CreateToken(Guid id, string name, string mail);

        public string EncryptPassword(string password);

        public string GetClaim(string token, string claimType);

        public string GetClaim(List<Claim> claims, string claimType);
    }
}
