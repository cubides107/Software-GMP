using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Domain.Validations;
using GMP.Users.Domain.Validations;
using System;

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
        /// for EF
        /// </summary>
        private User():base()
        {

        }

        /// <summary>
        /// constructor para el registro de un usuario
        /// constructor para el loqueo de un usuario
        /// constructor para la actualizacion del usuario
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
            string address, TipoUsuarioEnum tipoUsuarioEnumGuid, string token = null, Guid ? id = null) :base(id)
        {
            this.Name = name;
            this.Lastname = lastname;
            this.Phone = phone;
            this.Password = password;
            this.Address = address;
            this.Mail = mail;
            this.TipoUsuario = tipoUsuarioEnumGuid;
            this.Token = token;

            //validacion do to
            var validations = Validator.Validate<User>(this, UserValidation.validations);

            if (validations.Count != 0)
            {
                string message = InvalidationsEntityArgumentsException.MESSAGE;
                validations.ForEach((x) => {
                    message += x + " ";
                });

                throw new InvalidationsEntityArgumentsException(message);
            }
        }

        /// <summary>
        /// constructor para crear un usuario root
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <param name="id"></param>
        internal User(string mail, string password, string token, Guid? id = null) : base(id)
        {
            this.Mail = mail;
            this.Password = password;
            this.TipoUsuario = TipoUsuarioEnum.root;
            this.Token = token;
            this.Phone = String.Format("ROOT");
            this.Name = String.Format("ROOT");
            this.Lastname = String.Format("ROOT");
            this.Address = String.Format("ROOT");

            //validacion do to
            var validations = Validator.Validate<User>(this, RegisterUserRootValidation.validations);

            if (validations.Count != 0)
            {
                string message = InvalidationsEntityArgumentsException.MESSAGE;
                validations.ForEach((x) => {
                    message += x + " ";
                });

                throw new InvalidationsEntityArgumentsException(message);
            }
        }
    }
}
