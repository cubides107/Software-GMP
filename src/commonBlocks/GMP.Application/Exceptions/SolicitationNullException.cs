using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class SolicitationNullException : Exception
    {
        public const string MESSAGE = "la solicitud de investigacion no puede ser nulo";
        public SolicitationNullException(string message): base(message)
        {

        }
    }
}
