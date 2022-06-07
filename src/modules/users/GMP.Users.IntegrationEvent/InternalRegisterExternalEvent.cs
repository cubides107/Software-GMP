using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.IntegrationEvent
{
    public class InternalRegisterExternalEvent : UserEvent
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Token { get; set; }
        
    }
}
