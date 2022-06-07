using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMP.Apis.Web.Exceptions
{
    public class ModelStateIsInvalid : Exception
    {
        public const string MESSAGE_MODEL_IS_INVALID = "El modelo es invalido";

        public ModelStateIsInvalid(string message): base(message)
        {

        }
    }
}
