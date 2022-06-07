using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Exceptions
{
    public class EnvironmentVariableException : Exception
    {
        public const string CLAVE_SEGURIDAD = "la clave de seguridad nunca se instancio...";

        public const string DOMINIO = "el dominio para el token nunca se instancio...";

        public const string TOKEN = "los dias de expiracion para el token nunca se instancio...";

        public EnvironmentVariableException(string message):base(message)
        {

        }
    }
}
