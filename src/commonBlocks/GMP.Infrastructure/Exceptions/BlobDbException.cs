using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Exceptions
{
    public class BlobDbException : Exception
    {
        public const string MESSAGE_NOT_SAVE = "no se guardo el archivo en la db ";

        public BlobDbException(string message): base(message)
        {

        }
    }
}
