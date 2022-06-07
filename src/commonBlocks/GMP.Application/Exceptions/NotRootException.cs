using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class NotRootException : Exception
    {
        public const string MESSAGE_MAIL = "el correo del usuario no corresponde a uno root";

        public const string MESSAGE_PASSWORD = "la contraseña no corresponde a una root";

        public const string MESSAGE_DB_PASSWORD_NULL = "el root no existe en la DB con la contraseña suministrada";

        public NotRootException(string message):base(message)
        {

        }
    }
}
