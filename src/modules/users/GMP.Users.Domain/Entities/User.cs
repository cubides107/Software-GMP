using GMP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; internal set; }

        public string Lastname { get; internal set; }

        public string Phone { get; internal set; }

        public string Mail { get; internal set; }

        public string Password { get; internal set; }

        public string Address { get; internal set; }

        public string Token { get; internal set; }

        public TipoUsuarioEnum TipoUsuario {get; internal set;}

        /// <summary>
        /// constructor para el registro de un usuario interno
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <param name="phone"></param>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <param name="address"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        internal User(string name, string lastname, string phone, string mail, string password, 
            string address, string token, Guid? id = null) :base(id)
        {
            //validacion do to

            this.Name = name;
            this.Lastname = lastname;
            this.Phone = phone;
            this.Password = password;
            this.Address = address;
            this.Mail = mail;
            this.TipoUsuario = TipoUsuarioEnum.interno;
            this.Token = token;
        }
    }
}
