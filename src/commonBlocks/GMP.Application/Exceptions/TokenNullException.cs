using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class TokenNullException : Exception
    {
        public const string MESSAGE = "El usuario ya ha Cerrado Sesión";
        public const string NOT_START_SESSION_EXCEPTION = "El Usuario no ha iniciado sesión";

        public TokenNullException(string message) : base(message)
        {

        }
    }
}
