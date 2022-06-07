using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public const string MESSAGE_INCORRECT_PASSWORD = "Contraseña Incorrecta";
        public IncorrectPasswordException(String message): base(message)
        {

        }
    }
}
