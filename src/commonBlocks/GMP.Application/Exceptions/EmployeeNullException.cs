using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
    public class EmployeeNullException : Exception
    {
        public const string MESSAGE = "el empleado no puede ser nulo";

        public EmployeeNullException(string message): base(message)
        {

        }
    }
}
