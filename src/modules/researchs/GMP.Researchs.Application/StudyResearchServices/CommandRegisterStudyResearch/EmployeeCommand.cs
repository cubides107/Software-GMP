using GMP.Researchs.Domain.Entities;

namespace GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch
{
    public class EmployeeCommand
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public TipoDocumentoEnum DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Specialty { get; set; }
        public string Post { get; set; }
        public string LandLine { get; set; }
        public string Phone { get; set; }
        public string FamilyCellPhone { get; set; }
        public bool IsCritical { get; set; }
        public string Company { get; set; }
    }

}
