using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain
{
    public interface IUserSecurity
    {
        public const string MAIL = "mail";

        public const string USERNAME = "Username";

        public const string USERID = "UserId";

        public const string JTI = "jti";

        public string CreateToken(Guid id, string name, string mail, string role);

        public string EncryptPassword(string password);

        public string GetClaim(string token, string claimType);

        public string GetClaim(List<Claim> claims, string claimType);
    }
}
