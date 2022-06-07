using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Exceptions
{
    public class AutoMappingException : Exception
    {
        public const string MESSAGE = "no se pudo mapear la entidad: ";

        public AutoMappingException(string message): base(message)
        {

        }
    }
}
