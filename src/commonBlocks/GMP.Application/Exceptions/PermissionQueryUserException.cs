using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class PermissionQueryUserException : Exception
    {
        public const string PERMISSION_REFUSE_MESSAGE = "Usuario Externo, Permiso denegado";
        public PermissionQueryUserException(string message) : base(message)
        {

        }
    }
}
