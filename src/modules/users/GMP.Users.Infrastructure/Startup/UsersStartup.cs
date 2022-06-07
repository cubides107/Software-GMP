using GMP.Users.Infrastructure.EFContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; //getConnectionString
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using GMP.Users.Application.UsersServices.CommandLoginUser;
using GMP.Users.Application.UsersServices.CommandInternalRegisterExternal;
using System.Reflection;
using GMP.Users.Application.UsersRootServices.CommandRootRegisterUserExternal;
using GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal;
using GMP.Users.Application.UsersRootServices.CommandAccessRoot;

namespace GMP.Users.Infrastructure.Startup
{
    public class UsersStartup
    {
        public static void SetUp(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);
            ConfigureIOC(services);
            ConfigureMediador(services);
            ConfigureMapper(services);
        }

        private static void ConfigureContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersDBContext>(options =>
            options.UseSqlServer(
                    configuration.GetConnectionString("GMPConnectionString")));
        }

        private static void ConfigureIOC(IServiceCollection services)
        {
            InyectionContainer.Inyection(services);
        }

        private static void ConfigureMediador(IServiceCollection services)
        {
            services.AddMediatR(typeof(LoginUserCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(InternalRegisterExternalCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RegisterUserExternalCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RegistersUserInternalCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AccessRootCommand).GetTypeInfo().Assembly);
        }

        private static void ConfigureMapper(IServiceCollection services)
        {
            //mapeo de entidades
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
