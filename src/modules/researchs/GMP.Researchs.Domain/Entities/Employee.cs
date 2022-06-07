using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Domain.Validations;
using GMP.Researchs.Domain.Validations;
using System;

namespace GMP.Researchs.Domain.Entities
{
    public class Employee : Entity
    {
        public string Name { get; internal set; }
        public string LastName { get; internal set; }
        public TipoDocumentoEnum DocumentType { get; internal set; }
        public string DocumentNumber { get; internal set; }
        public string Specialty { get; internal set; }
        public string Post { get; internal set; }
        public string LandLine { get; internal set; }
        public string Phone { get; internal set; }
        public string FamilyCellPhone { get; internal set; }
        public bool IsCritical { get; internal set; }
        public string Company { get; internal set; }

        public Solicitation Solicitation { get; internal set; }

        /// <summary>
        /// for EF
        /// </summary>
        private Employee() : base()
        {

        }

        /// <summary>
        /// constructor para crear un empleado
        /// </summary>
        internal Employee(string name, string lastName, TipoDocumentoEnum documentType, string documentNumber,
            string specialty, string post, string landLine, string phone, string familyCellPhone,
            bool isCritical, string company, Solicitation solicitation = null, Guid? id = null) : base(id)
        {
            this.Name = name; //obligatorio
            this.LastName = lastName; //obligatorio
            this.DocumentType = documentType; //obligatorio
            this.DocumentNumber = documentNumber; //obligatorio
            this.Specialty = specialty;
            this.Post = post;
            this.LandLine = landLine;
            this.Phone = phone; //obligatorio
            this.FamilyCellPhone = familyCellPhone;
            this.Solicitation = solicitation; 
            this.IsCritical = isCritical; //obligatorio
            this.Company = company; //obligatorio

            //validacion do to
            var validations = Validator.Validate<Employee>(this, RegisterEmployeeValidation.validations);

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
