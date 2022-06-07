using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Exceptions
{
    public class EncriptPasswordException : Exception
    {
        public const string CONTRASEÑA = "la contraseña no puede ser nula o vacia para encriptarla";

        public const string CARACTERES_CONTRASEÑA = "la contraseña debe tener al menos un digito, una mayuscula y una minuscula";

        public const string NUEVE_CARACTERES = "la contraseña debe ser mayor o igual a 9 caracteres";

        public EncriptPasswordException(string message):base(message)
        {

        }
    }
}
