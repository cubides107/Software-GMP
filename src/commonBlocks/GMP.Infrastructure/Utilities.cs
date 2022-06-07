using GMP.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure
{
    public class Utilities : IUtilities
    {
        private readonly IConfiguration configuration;

        public Utilities(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Guid CreateId()
        {
            return Guid.NewGuid();
        }

        public Guid CreateId(string id)
        {
            //falta velidar el id que corresponda al formato y a la cantidad de caracteres
            return Guid.Parse(id);
        }

        public string GetEnvironmentVariable(string name)
        {
            return this.configuration[name];
        }
    }
}
