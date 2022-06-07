using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch
{
    public partial class RegisterStudyResearchCommand : IRequest<RegisterStudyResearchDTO>
    {
        public SolicitationCommand Solicitation { get; set; }

        public EmployeeCommand Employee { get; set; }

        public AddressCommand Address { get; set; }

        public IFormFile File { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
