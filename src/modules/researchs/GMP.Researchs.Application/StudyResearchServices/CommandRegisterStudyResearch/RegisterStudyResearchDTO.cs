using GMP.Application;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch
{
    public class RegisterStudyResearchDTO : DTO<RegisterStudyResearchDTO>
    {
        public string UserId { get; set; }

        public string UserMail { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string SolicitationId { get; set; }

        public DateTime SolicitationStartDate { get; set; }

        public DateTime SolicitationEndDate { get; set; }

        public JornadaEnum SolicitationJourney { get; set; }
    }
}
