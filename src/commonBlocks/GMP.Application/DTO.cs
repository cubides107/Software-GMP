using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Application
{
    /// <summary>
    /// clase abstracta que representa el DTO
    /// su utilidad es identificar que tipo de mensaje se esta enviando
    /// solo puede ser eredada de clases que su objetivo sea trasferir datos al fron end
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DTO<T>
    {
        /// <summary>
        /// atributo que representa el tipo de objeto que es el DTO
        /// </summary>
        public string MessageType { get { return typeof(T).FullName; } }
    }
}
