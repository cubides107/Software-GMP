using GMP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Entities
{
    /// <summary>
    /// clase concreta que representa la enumeracion del tipo de solicitud
    /// esta clase provee de un campo que sera mapeado en la db para poder guardar
    /// una lista de registrps del tipo de enumeradores,
    /// es decir, representa una tabla que tendra una columna y contendra los valores del enumerador
    /// se crea de esta manera para que tenga una relacion uno a muchos con las demas clases,
    /// ya que, no esta permitido tener una relacion 1- n con enumeradores
    /// </summary>
    public class TipoSolicitudEnum : Entity
    {
        public TipoEnum Tipo { get; set; }

        /// <summary>
        /// atributo que representa la clave foranea
        /// </summary>
        public string SolicitationId { get; set; }

        /// <summary>
        /// for ef
        /// </summary>
        public TipoSolicitudEnum():base()
        {
        }

        public enum TipoEnum
        {
            estudioDeSeguridad = 0,
            visitaDomiciliaria = 1,
            verificaciónAcadémica = 2,
            verificaciónLaboral = 3,
            análisisFinanciero = 4,
            listasRestrictivas = 5,
            referenciaPersonal = 6,
            antecedentes = 7
        }
    }
}
