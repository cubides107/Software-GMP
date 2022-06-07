using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class UserExistsException : Exception
    {
        public const string MESSAGE = "EL USUARIO YA EXISTE.";
        public const string MESSAGE_EXISTS_USER_MAIL = "El nuevo correo del usuario ya existe";

        public UserExistsException(string message):base(message)
        {

        }
    }
}
