using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class CommandRequestNullException : Exception
    {
        public const string MESSAGE_REQUEST_ISNULL = "La peticion esta nula";

        public CommandRequestNullException(string message) : base(message) 
        { 
        }
    }
}
