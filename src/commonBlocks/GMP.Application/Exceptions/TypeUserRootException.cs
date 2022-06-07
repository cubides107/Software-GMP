using System;

namespace GMP.Application.Exceptions
{
    public class TypeUserRootException : Exception
    {
        public const string MESSAGE_TYPE_USER_IS_ROOT = "El usuario a editar no puede ser de tipo root";
        public const string MESSAGE_TYPE_USER_ROOT_RESTORE_PASSWORD = "La contraseña no se pude modificar al usuario root";

        public TypeUserRootException(string message): base(message)
        {

        }
    }
}
