using GMP.Domain;
using GMP.Infrastructure.Security;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Infrastructure.Startup
{
    public class InyectionContainer
    {
        public static void Inyection(IServiceCollection services)
        {
            services.AddScoped<IUsersRepositories, UsersRepositorySQL>();
            services.AddScoped<IUsersFactories, UsersFactories>();
        }
    }
}
