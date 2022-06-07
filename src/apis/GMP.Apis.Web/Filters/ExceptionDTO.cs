using GMP.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMP.Apis.Web.Filters
{
    internal class ExceptionDTO : DTO<ExceptionDTO>
    {
        public string Mensaje { get; set; }

        public string Tipo { get; set; }

        public string Aplicacion { get; set; }
    }
}
