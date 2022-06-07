using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Exceptions
{
    public class SqlDbException : Exception
    {
        public const string MESSAGE_NOT_EXIST = "no se pudo verificiar si existe la entidad ";

        public const string MESSAGE_NOT_SAVE = "no se guardo la entidad en la db ";

        public const string MESSAGE_NOT_UPDATE = "no se actualizo la entidad en la db ";

        public const string MESSAGE_NOT_GET = "no se pudo recuperar la entidad";

        public const string MESSAGE_NOT_GET_QUERY_LIST = "no se logro recuperar la lista de usuarios";

        public SqlDbException(string message):base(message)
        {

        }
    }
}
