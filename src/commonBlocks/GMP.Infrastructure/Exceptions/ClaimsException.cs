using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Exceptions
{
    public class ClaimsException : Exception
    {
        public const string NULOS = "los claims son nulos o vacios";

        public const string EXISTE = "el nombre o tipo de claim que esta enviando no existe";

        public const string TOKEN = "el token es nulo";

        public ClaimsException(string message):base(message)
        {

        }
    }
}
