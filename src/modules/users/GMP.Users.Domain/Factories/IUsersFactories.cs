using GMP.Domain;
using GMP.Users.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Domain.Factories
{
    public interface IUsersFactories
    {
        /// <summary>
        /// construye un usuario para registralo
        /// construye un usuario para loquear
        /// construye un usuario para actualizar
        /// </summary>
        /// <returns></returns>
        public Entity BuildUser(string name, string lastname, string phone, string mail, string password,
            string address, TipoUsuarioEnum tipoUsuarioEnumGuid, string token = null, Guid? id = null);
      
        /// construye un usuario root
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity BuilRegisterUserRoot(string mail, string password, string token, Guid? id = null);

      
    }
}
