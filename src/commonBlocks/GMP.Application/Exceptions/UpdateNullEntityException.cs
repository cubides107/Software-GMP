using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application.Exceptions
{
   public class UpdateNullEntityException: Exception
   {
        public const string MESSAGE_ENTITY_IS_NULL = "Retorno nulo al actualizar el cliente";

        public UpdateNullEntityException(string message) : base(message)
        {

        }
    
   }

}
