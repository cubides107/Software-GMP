using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Researchs.Domain.Entities;
using GMP.Researchs.Domain.Factories;
using GMP.Researchs.Domain.Ports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch
{
    public class RegisterStudyResearchHandler : IRequestHandler<RegisterStudyResearchCommand, RegisterStudyResearchDTO>
    {
        private readonly IResearchsRepository researchsRepository;

        private readonly IResearchsRepositoryBlob researchsRepositoryBlob;

        private readonly IResearchsFactory researchsFactory;

        private readonly IUtilities utilities;

        private readonly IUserSecurity userSecurity;

        public RegisterStudyResearchHandler(IResearchsRepository researchsRepository, IResearchsRepositoryBlob researchsRepositoryBlob,
            IResearchsFactory researchsFactory, IUtilities utilities, IUserSecurity userSecurity)
        {
            this.researchsRepository = researchsRepository;
            this.researchsRepositoryBlob = researchsRepositoryBlob;
            this.researchsFactory = researchsFactory;
            this.utilities = utilities;
            this.userSecurity = userSecurity;
        }

        public async Task<RegisterStudyResearchDTO> Handle(RegisterStudyResearchCommand request, CancellationToken cancellationToken)
        {
            string userId;
            Address address = null;
            MapHome mapHome = null;
            Solicitation solicitation;
            User UserCreatesSolicitation;
            Employee employee;
            string nameFile = null;
            string blobName = null;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //0. verificar si el usuario que realiza esta accion esta registrado y lo obtenemos
            userId = this.userSecurity.GetClaim(request.Claims, IUserSecurity.USERID);

            if (this.researchsRepository.Exists<User>(x => x.Id == userId) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            UserCreatesSolicitation = await this.researchsRepository.Get<User>(x => x.Id == userId, cancellationToken); 

            //1. si es requerido creamos la direccion
            if(request.Address is not null)
            {
                address =(Address) this.researchsFactory.BuildRegisterAddress(
                    neighborhood: request.Address.Neighborhood,
                    addressText: request.Address.AddressText, 
                    city: request.Address.City, 
                    country: request.Address.Country );
            }

            //2. si es requerido creamos el registro del mapa
            if (request.File is not null)
            {
                nameFile = utilities.CreateId().ToString() + ".pdf";
                blobName = utilities.GetEnvironmentVariable(IUtilities.NAME_BLOB);

                mapHome = (MapHome)this.researchsFactory.BuildRegisterMapHome(blobName, nameFile);
            }

            //3. es necesario crear un empleado y solicitud, por lo tanto, comprovamos que el request lo contenga
            if (request.Employee is null)
                throw new EmployeeNullException(EmployeeNullException.MESSAGE);

            else if (request.Solicitation is null)
                throw new SolicitationNullException(SolicitationNullException.MESSAGE);

            //4. creamos el empleado
            employee = (Employee) this.researchsFactory.BuildRegisterEmployee(
                name: request.Employee.Name, 
                lastName: request.Employee.LastName,
                documentType: request.Employee.DocumentType, 
                documentNumber: request.Employee.DocumentNumber, 
                specialty: request.Employee.Specialty,
                post: request.Employee.Post, 
                landLine: request.Employee.LandLine, 
                phone: request.Employee.Phone, 
                familyCellPhone: request.Employee.FamilyCellPhone,
                isCritical: request.Employee.IsCritical,
                company: request.Employee.Company);

            //5. creamos la solicitud con todos los objetos asociados y objetemos los valores para la lista de enumeradores de tipo solicitud
            List<TipoSolicitudEnum> tipoSolicitudEnums = new();

            request.Solicitation.TipoSolicitudEnums.ForEach((x) => {
                tipoSolicitudEnums.Add(new TipoSolicitudEnum { Tipo = Enum.Parse<TipoSolicitudEnum.TipoEnum>(x.ToString()) });
            });

            solicitation = (Solicitation)this.researchsFactory.BuildRegisterSolicitation(
                solicitationDate: request.Solicitation.SolicitationDate,
                startDate: request.Solicitation.StartDate, 
                endDate: request.Solicitation.EndDate, 
                journey: request.Solicitation.Journey,
                employee: employee, 
                address: address, 
                mapHome: mapHome, 
                userCreatesSolicitation: UserCreatesSolicitation,
                tipoSolicitudEnums: tipoSolicitudEnums,
                tipoVisitaEnum: request.Solicitation.TipoVisitaEnum);

            //6. guardamos la solicitud con todos los objetos asociados 
            solicitation = await this.researchsRepository.Save<Solicitation>(solicitation, cancellationToken);
            if (solicitation is null)
                throw new SaveNullException(SaveNullException.MESSAGE);

            //7. si es necesario guardamos en el storage el mapa
            if (request.File is not null)
                await this.researchsRepositoryBlob.UploadFile(blobName, nameFile, request.File, cancellationToken);

            //8. retornamos los datos esenciales del registro de estudio de investigacion de un empleado
            return new RegisterStudyResearchDTO
            {
                UserId = UserCreatesSolicitation.Id,
                UserMail = UserCreatesSolicitation.Mail,
                EmployeeId = employee.Id,
                EmployeeName = employee.Name,
                SolicitationId = solicitation.Id,
                SolicitationStartDate = solicitation.StartDate,
                SolicitationEndDate = solicitation.EndDate,
                SolicitationJourney = solicitation.Journey
            };
        
        }
    }
}
