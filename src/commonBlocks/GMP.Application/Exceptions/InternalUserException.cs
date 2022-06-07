using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class InternalUserException : Exception
    {
        public const string MESSAGE = "NO ES UN USUARIO INTERNO.";

        public InternalUserException(string message): base(message)
        {

        }
    }
}
