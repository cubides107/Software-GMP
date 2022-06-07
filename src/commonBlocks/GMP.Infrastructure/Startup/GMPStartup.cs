using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Startup
{
    public class GMPStartup
    {
        public static void SetUp(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureIOC(services);
        }

        private static void ConfigureIOC(IServiceCollection services)
        {
            InyectionContainer.Inyection(services);
        }
    }
}
