using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Domain.Validations;
using GMP.Researchs.Domain.Validations;

namespace GMP.Researchs.Domain.Entities
{
    public class Address : Entity
    {
        public string Neighborhood { get; internal set; }
        public string AddressText { get; internal set; }
        public string City { get; internal set; }
        public string Country { get; internal set; }
        public Solicitation Solicitation { get; internal set; }

        /// <summary>
        /// for EF
        /// </summary>
        private Address()
        {

        }

        internal Address(string neighborhood, string addressText, string city, string country, Solicitation solicitation = null)
        {
            this.Neighborhood = neighborhood; //obligatorio
            this.AddressText = addressText; //obligatorio
            this.City = city; //obligatorio
            this.Country = country; //obligatorio
            this.Solicitation = solicitation;

            //validacion do to
            var validations = Validator.Validate<Address>(this, RegisterAddressValidation.validations);

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
