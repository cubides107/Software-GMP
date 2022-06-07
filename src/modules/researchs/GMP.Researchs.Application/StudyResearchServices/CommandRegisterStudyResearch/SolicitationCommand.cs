using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;

namespace GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch
{
    public class SolicitationCommand
    {
        public DateTime SolicitationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public JornadaEnum Journey { get; set; }
        public List<int> TipoSolicitudEnums { get; set; }
        public TipoVisitaEnum TipoVisitaEnum { get; set; }
    }

}
