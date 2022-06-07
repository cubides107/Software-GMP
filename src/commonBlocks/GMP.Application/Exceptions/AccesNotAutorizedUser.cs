using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class AccesNotAutorizedUser: Exception
    {
        public const string MESSAGE_ACCESS_DENIED = "El usuario no puede realizar esta Accion, ya que es un usuario externo";
    
        public AccesNotAutorizedUser(string message) : base(message)
        {

        }
    }
}
