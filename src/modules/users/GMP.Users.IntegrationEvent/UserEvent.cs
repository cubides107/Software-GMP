using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.IntegrationEvent
{
    public abstract class UserEvent
    {
        public string Id { get; set; }

        public string Mail { get; set; }

        public int TipoUsuario { get; set; }
    }
}
