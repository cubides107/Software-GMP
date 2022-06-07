using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class UserNotRegisterException : Exception
    {
        public const string MESSAGE_USER_IS_NOT_REGISTER = "El usuario no esta registrado";
        public const string MESSAGE_USER_TO_EDIT_IS_NOT_REGISTER = "El usuario que desea editar no esta registrado";
        public UserNotRegisterException(string message) : base(message)
        {

        }
    }
}
