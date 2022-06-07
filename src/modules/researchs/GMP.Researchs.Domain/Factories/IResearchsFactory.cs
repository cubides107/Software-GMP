using GMP.Domain;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Factories
{
    public interface IResearchsFactory
    {
        /// <summary>
        /// crea el registro de la solictud con relaciones de ids
        /// </summary>
        /// <param name="solicitationDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="journey"></param>
        /// <param name="employeeId"></param>
        /// <param name="addressId"></param>
        /// <param name="mapHomeId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity BuildRegisterSolicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, JornadaEnum journey,
            string employeeId, string addressId, string mapHomeId, string userCreatesSolicitationId,
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null);

        /// <summary>
        /// crea el registro de solicitud con relaciones de objetos
        /// </summary>
        /// <param name="solicitationDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="journey"></param>
        /// <param name="employeeId"></param>
        /// <param name="addressId"></param>
        /// <param name="mapHomeId"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity BuildRegisterSolicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, 
            JornadaEnum journey, Employee employee, Address address, MapHome mapHome, User userCreatesSolicitation, 
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum);

        /// <summary>
        /// crea el registro del empleado
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="documentType"></param>
        /// <param name="documentNumber"></param>
        /// <param name="specialty"></param>
        /// <param name="post"></param>
        /// <param name="landLine"></param>
        /// <param name="phone"></param>
        /// <param name="familyCellPhone"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity BuildRegisterEmployee(string name, string lastName, TipoDocumentoEnum documentType, string documentNumber,
            string specialty, string post, string landLine, string phone, string familyCellPhone,
            bool isCritical, string company, Solicitation solicitation = null, Guid ? id = null);

        /// <summary>
        /// crea el registro de la direccion
        /// </summary>
        /// <param name="neighborhood"></param>
        /// <param name="addressText"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public Entity BuildRegisterAddress(string neighborhood, string addressText, string city, string country, 
            Solicitation solicitation = null);

        /// <summary>
        /// crea el registro de la ruta de la imagen del mapa
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="label"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity BuildRegisterMapHome(string containerName, string label, Solicitation solicitation = null, 
            Guid ? id = null);

        /// <summary>
        /// crea el regitro del usuario 
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity BuildRegisterUser(string mail, Guid? id = null);

        public Entity BuildSolicitationAssignment(DateTime solicitationDate, DateTime startDate, DateTime endDate, JornadaEnum journey,
            string employeeId, string addressId, string mapHomeId, string userCreatesSolicitationId, string userManagesSolicitationId,
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null);
    }
}
