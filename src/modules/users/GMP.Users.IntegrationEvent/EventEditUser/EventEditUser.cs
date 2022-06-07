using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.IntegrationEvent.EventEditUser
{
    public class EventEditUser
    {
        public string Id { get; set; }

        public string Mail { get; set; }
        
        public string Name { get; set; }

        public string Lastname { get; set; }

        public int TipoUsuario { get; set; }

        public string Token { get; set; }
    }
}
