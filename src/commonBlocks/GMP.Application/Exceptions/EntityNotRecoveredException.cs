using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
   public class EntityNotRecoveredException : Exception
    {
        public const string MESSAGE_USER_NOT_RECOVERED = "Usuario no recuperado";
        public EntityNotRecoveredException(string message) : base(message)
        {

        }
    }
}
