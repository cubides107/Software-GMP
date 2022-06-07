using GMP.Domain;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Factories
{
    public class ResearchsFactory : IResearchsFactory
    {
        public Entity BuildRegisterSolicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, JornadaEnum journey,
            string employeeId, string addressId, string mapHomeId, string UserCreatesSolicitationId, 
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null)
        {
            return new Solicitation(solicitationDate, startDate, endDate, journey, employeeId, addressId, mapHomeId, UserCreatesSolicitationId, tipoSolicitudEnums, tipoVisitaEnum, id);
        }

        public Entity BuildRegisterSolicitation(DateTime solicitationDate, DateTime startDate, DateTime endDate, 
            JornadaEnum journey, Employee employee, Address address, MapHome mapHome, User UserCreatesSolicitation, 
            List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum)
        {
            return new Solicitation(solicitationDate, startDate, endDate, journey, employee, address, mapHome, 
                UserCreatesSolicitation, tipoSolicitudEnums, tipoVisitaEnum);
        }

        public Entity BuildRegisterEmployee(string name, string lastName, TipoDocumentoEnum documentType, string documentNumber, 
            string specialty, string post, string landLine, string phone, string familyCellPhone,
            bool isCritical, string company, Solicitation solicitation = null, Guid ? id = null)
        {
            return new Employee(name, lastName, documentType, documentNumber, specialty, 
                post, landLine, phone, familyCellPhone, isCritical, company, solicitation, id);
        }

        public Entity BuildRegisterAddress(string neighborhood, string addressText, string city, string country, Solicitation solicitation = null)
        {
            return new Address(neighborhood, addressText, city, country, solicitation);
        }

        public Entity BuildRegisterMapHome(string containerName, string label, Solicitation solicitation = null, Guid? id = null)
        {
            return new MapHome(containerName, label, solicitation, id);
        }

        public Entity BuildRegisterUser(string mail, Guid? id = null)
        {
            return new User(mail, id);
        }

        public Entity BuildSolicitationAssignment(DateTime solicitationDate, DateTime startDate, DateTime endDate, 
            JornadaEnum journey, string employeeId, string addressId, string mapHomeId, string userCreatesSolicitationId,
            string userManagesSolicitationId, List<TipoSolicitudEnum> tipoSolicitudEnums, TipoVisitaEnum tipoVisitaEnum, Guid? id = null)
        {
            return new Solicitation(solicitationDate, startDate, endDate, journey, employeeId, addressId, mapHomeId, 
                userCreatesSolicitationId, userManagesSolicitationId, tipoSolicitudEnums, tipoVisitaEnum, id);
        }
    }
}
