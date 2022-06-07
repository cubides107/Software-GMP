using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain
{
    public interface IUtilities
    {
        public const string CLAVE_SECRETA = "CLAVE_SECRETA";

        public const string DOMINIO_APP = "DOMINIO_APP";

        public const string DIAS_EXPIRACION = "DIAS_EXPIRACION";

        public const string MAIL_USER_ROOT = "MAIL_USER_ROOT";

        public const string NAME_USER_ROOT = "NAME_USER_ROOT";

        public const string PASSWORD_USER_ROOT = "PASSWORD_USER_ROOT";

        public const string NAME_BLOB = "NAME_BLOB";

        /// <summary>
        /// crear el id
        /// </summary>
        /// <returns></returns>
        public Guid CreateId();

        /// <summary>
        /// Convierte y crea el Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid CreateId(string id);


        /// <summary>
        /// retorna una variable de entorno segun el nombre 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetEnvironmentVariable(string name);
    }
}
