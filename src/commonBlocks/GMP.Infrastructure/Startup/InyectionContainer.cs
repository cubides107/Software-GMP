using GMP.Domain;
using GMP.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure.Startup
{
    public class InyectionContainer
    {
        public static void Inyection(IServiceCollection services)
        {
            services.AddScoped<IUtilities, Utilities>();
            services.AddScoped<IAutoMapping, AutoMapping>();
            services.AddScoped<IUserSecurity, UsersSecurity>();
        }
    }
}
