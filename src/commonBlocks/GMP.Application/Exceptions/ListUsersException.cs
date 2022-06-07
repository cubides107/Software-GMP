using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class ListUsersException : Exception
    {
        public const string MESSAGE_LIST_NULL = "La lista esta vacia";
        public const string MESSAGE_LIST_EMPTY = "La lista esta vacia";

        public ListUsersException(string message):base(message)
        {

        }

    }
}
