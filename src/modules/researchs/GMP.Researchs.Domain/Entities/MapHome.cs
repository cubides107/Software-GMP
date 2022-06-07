using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Domain.Validations;
using GMP.Researchs.Domain.Validations;
using System;

namespace GMP.Researchs.Domain.Entities
{
    public class MapHome : Entity
    {
        public string ContainerName { get; internal set; }
        public string Label { get; internal set; }

        public Solicitation Solicitation { get; internal set; }

        /// <summary>
        /// for EF
        /// </summary>
        private MapHome()
        {
        }

        /// <summary>
        /// creacion de la clase para la ruta de la imagen
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="label"></param>
        internal MapHome(string containerName, string label, Solicitation solicitation = null, Guid? id = null): base(id)
        {
            this.ContainerName = containerName;
            this.Label = label;
            this.Solicitation = solicitation;

            //validacion do to
            var validations = Validator.Validate<MapHome>(this, RegisterMapHomeValidation.validations);

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
