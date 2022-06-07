using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GMP.Infrastructure.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using GMP.Domain;

namespace GMP.Infrastructure.Security
{
    public class UsersSecurity : IUserSecurity
    {
        private readonly IUtilities utilities;

        public UsersSecurity(IUtilities utilities)
        {
            this.utilities = utilities;
        }


        public string CreateToken(Guid id, string name, string mail, string role)
        {
            //creamos el cleims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, mail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Username", name),
                new Claim("UserId", id.ToString()),
                new Claim("mail", mail),
                new Claim("role", role)
            };

            //obtener variables de entorno
            string secret = this.utilities.GetEnvironmentVariable(IUtilities.CLAVE_SECRETA);
            string domain = this.utilities.GetEnvironmentVariable(IUtilities.DOMINIO_APP);
            string dayString = this.utilities.GetEnvironmentVariable(IUtilities.DIAS_EXPIRACION);
            int dayExpires = int.Parse(dayString);

            //verificar variables de entorno
            if (secret == null || secret == "")
                throw new EnvironmentVariableException(EnvironmentVariableException.CLAVE_SEGURIDAD);
            else if (domain == null || domain == "")
                throw new EnvironmentVariableException(EnvironmentVariableException.DOMINIO);
            else if (dayString == null || dayString == "")
                throw new EnvironmentVariableException(EnvironmentVariableException.TOKEN);

            //encriptar clave secreta,crear credenciales y dias de expiracion
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issues = DateTime.UtcNow.AddDays(dayExpires); //expide

            //creamos el token con los datos anteriores
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: domain,
                audience: domain,
                claims: claims,
                expires: issues,//expide
                signingCredentials: credential
                );

            //retornamos el token tipo string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string EncryptPassword(string password)
        {
            //verificar contraseña
            ValidationsSegurity.Validator(password, ValidationsSegurity.checksPassword);

            //instancias variables
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream;
            StringBuilder sb = new StringBuilder();

            //encriptar
            stream = sha1.ComputeHash(encoding.GetBytes(password));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            return sb.ToString();
        }

        public string GetClaim(List<Claim> claims, string claimType)
        {
            ValidationsSegurity.Validator(claimType, ValidationsSegurity.checksClaims);

            var claim = claims.First(claim => claim.Type == claimType).Value;
            return claim;
        }

        public string GetClaim(string token, string claimType)
        {
            //validar
            ValidationsSegurity.Validator(token, ValidationsSegurity.checksToken);
            ValidationsSegurity.Validator(claimType, ValidationsSegurity.checksClaims);

            //crear token apartir del string
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            //obtener claim segun el parametro
            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}
