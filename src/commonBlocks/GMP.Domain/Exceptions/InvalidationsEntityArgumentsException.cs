using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain.Exceptions
{
    public class InvalidationsEntityArgumentsException : Exception
    {
        public const string MESSAGE = "no se puede crear el usuario, argumentos erroneos: ";

        public InvalidationsEntityArgumentsException(string message): base(message)
        {

        }
    }
}
