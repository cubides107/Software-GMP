using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Domain.Validations;
using GMP.Researchs.Domain.Validations;
using System;
using System.Collections.Generic;

namespace GMP.Researchs.Domain.Entities
{
    public class Solicitation : Entity
    {
        public DateTime SolicitationDate { get; internal set; }
        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }
        public JornadaEnum Journey { get; internal set; }
        public bool Reviewed { get; internal set; }
        public List<TipoSolicitudEnum> TipoSolicitudEnums { get; internal set; }
        public TipoVisitaEnum TipoVisitaEnum { get; internal set; }

        // Atributos para relacon con direccion
        public Employee Employee { get; internal set; }
        public string EmployeeId { get; internal set; }

        // Atributos para relacon con direccion
        public Address Address { get; internal set; }
        public string AddressId { get; internal set; }

        /// Atributos para relacion con mapa de vivienda
        public MapHome MapHome { get; internal set; }
        public string MapHomeId { get; internal set; }

        // Atributos para relacion con el usuario quien crea la solicitud
        public string UserCreatesSolicitationId { get; internal set; }
        public User UserCreatesSolicitation { get; internal set; }

        // Atributos para relacion con el usuario quien gestionara la solicitud
        public string UserManagesSolicitationId { get; internal set; }
        public User UserManagesSolicitation { get; internal set; }

        /// <summary>
        /// for Ef
        /// </summary>
        private Solicitation() : base()
        {

        }

        /// <summary>
        /// crear la solicitud con los ids de objetos relacionados
        /// </summary>
        /// <param name="solicitationDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="journey"></param>
        /// <param name="employeeId"></param>
        /// <param name="addressId"></param>
        /// <param name="mapHomeId"></param>
        /// <param name="userCreatesSolicitationId"></param>
        /// <param name="id"></param>
        internal Solicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, JornadaEnum journey,
            string employeeId, string addressId, string mapHomeId, string userCreatesSolicitationId,
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null) : base(id)
        {

            this.SolicitationDate = solicitationDate; //obligatorio
            this.StartDate = startDate; //obligatorio
            this.EndDate = endDate; //obligatorio
            this.Journey = journey; //obligatorio
            this.Reviewed = false;
            this.EmployeeId = employeeId; //obligatorio
            this.AddressId = addressId;
            this.MapHomeId = mapHomeId;
            this.UserCreatesSolicitationId = userCreatesSolicitationId; //obligatorio
            this.TipoSolicitudEnums = tipoSolicitudEnums; //obligatorio
            this.TipoVisitaEnum = tipoVisitaEnum; //obligatorio

            //validacion do to
            var validations = Validator.Validate<Solicitation>(this, RegisterSolicitationValidation.validations);

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
        /// crear la solicitud con los objetos relacionados
        /// </summary>
        /// <param name="solicitationDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="journey"></param>
        /// <param name="employee"></param>
        /// <param name="address"></param>
        /// <param name="mapHome"></param>
        /// <param name="userCreatesSolicitation"></param>
        /// <param name="id"></param>
        internal Solicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, JornadaEnum journey,
            Employee employee, Address address, MapHome mapHome, User userCreatesSolicitation,
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null) : base(id)
        {

            this.SolicitationDate = solicitationDate; //obligatorio
            this.StartDate = startDate; //obligatorio
            this.EndDate = endDate; //obligatorio
            this.Journey = journey; //obligatorio
            this.Reviewed = false;
            this.Employee = employee; //obligatorio
            this.Address = address;
            this.MapHome = mapHome;
            this.UserCreatesSolicitation = userCreatesSolicitation; //obligatorio
            this.TipoSolicitudEnums = tipoSolicitudEnums; //obligatorio
            this.TipoVisitaEnum = tipoVisitaEnum; //obligatorio

            //validacion do to
            var validations = Validator.Validate<Solicitation>(this, RegisterSolicitationValidation.validations);

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
        /// crear la solicitud con los ids de objetos relacionados y el usuario quien va a gestionar esta solicitud
        /// </summary>
        /// <param name="solicitationDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="journey"></param>
        /// <param name="employeeId"></param>
        /// <param name="addressId"></param>
        /// <param name="mapHomeId"></param>
        /// <param name="userCreatesSolicitationId"></param>
        /// <param name="userManagesSolicitationId"></param>
        /// <param name="id"></param>
        internal Solicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, JornadaEnum journey,
            string employeeId, string addressId, string mapHomeId, string userCreatesSolicitationId,
            string userManagesSolicitationId, List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null): base(id)
        {
            this.SolicitationDate = solicitationDate; //obligatorio
            this.StartDate = startDate; //obligatorio
            this.EndDate = endDate; //obligatorio
            this.Journey = journey; //obligatorio
            this.Reviewed = true;
            this.EmployeeId = employeeId; //obligatorio
            this.AddressId = addressId;
            this.MapHomeId = mapHomeId; 
            this.UserCreatesSolicitationId = userCreatesSolicitationId; //obligatorio
            this.UserManagesSolicitationId = userManagesSolicitationId; //obligatorio
            this.TipoSolicitudEnums = tipoSolicitudEnums; //obligatorio
            this.TipoVisitaEnum = tipoVisitaEnum; //obligatorio

            //validacion do to
            var validations = Validator.Validate<Solicitation>(this, RegisterSolicitationValidation.validationsWithUserManages);

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
