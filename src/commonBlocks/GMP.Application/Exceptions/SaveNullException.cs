using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class SaveNullException : Exception
    {
        public const string MESSAGE = "no se pudo guarda la entidad en la db"; 

        public SaveNullException(string message) : base(message)
        {

        }
    }
}
