using GMP.Domain;
using GMP.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Security
{
    internal static class ValidationsSegurity
    {
        internal static readonly Action<string>[] checksPassword =
        {
            (x) =>
            {
                var isCorrect = (x != null & x != "");
                if(!isCorrect)
                    throw new EncriptPasswordException(EncriptPasswordException.CONTRASEÑA);
            },

            (x) =>
            {
                var isCorrect = (x.Any(x => char.IsUpper(x)) &&  x.Any(x => char.IsLower(x)) &&
                                    x.Any(x => char.IsDigit(x)));

                if(!isCorrect)
                    throw new EncriptPasswordException(EncriptPasswordException.CARACTERES_CONTRASEÑA);
            },

            (x) =>
            {
                var isCorrect = (x.Length >= 9);
                if(!isCorrect)
                    throw new EncriptPasswordException(EncriptPasswordException.NUEVE_CARACTERES);
            }
        };

        internal static readonly Action<string>[] checksClaims =
        {
            (x) =>
            {
                var isCorrect = x != null && x!= "";
                if(!isCorrect)
                    throw new ClaimsException(ClaimsException.NULOS);
            },

            (x) =>
            {
                var isCorrect = (x == IUserSecurity.MAIL || x == IUserSecurity.USERNAME || 
                                 x == IUserSecurity.USERID || x == IUserSecurity.JTI);
                if(!isCorrect)
                    throw new ClaimsException(ClaimsException.EXISTE);
            }
        };

        internal static readonly Action<string>[] checksToken =
        {
            (x) =>
            {
                var isCorrect = x != null && x!= "";
                if(!isCorrect)
                    throw new ClaimsException(ClaimsException.TOKEN);
            },
        };

        internal static void Validator(string parameter, params Action<string>[] validations)
        {
            foreach (var validation in validations)
            {
                validation(parameter);
            }
        }
    }
}
