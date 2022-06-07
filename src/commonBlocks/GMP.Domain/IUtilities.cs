using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain
{
    public interface IUtilities
    {
        /// <summary>
        /// crear el id
        /// </summary>
        /// <returns></returns>
        public Guid CreateId();

        /// <summary>
        /// converte y crea el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid Createid(string id);
    }
}
