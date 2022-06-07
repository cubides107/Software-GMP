using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Domain.Validations;
using GMP.Researchs.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Entities
{
    public class User : Entity
    {
        public string Mail { get; internal set; }

        /// <summary>
        /// relacion para las solicitud que el usuario a creado
        /// </summary>
        public List<Solicitation> CreatedSolicitations { get; internal set; }

        /// <summary>
        /// relacion para las solicitudes que el usuario a gestionado
        /// </summary>
        public List<Solicitation> ManagedSolicitations { get; internal set; }


        /// <summary>
        /// for EF
        /// </summary>
        private User()
        {

        }

        /// <summary>
        /// constructor cuando resibimos el evento de que se creo un usuario
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="id"></param>
        internal User(string mail, Guid? id = null): base(id)
        {
            this.Mail = mail;

            //validacion do to
            var validations = Validator.Validate<User>(this, RegisterUserValidation.validations);

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
