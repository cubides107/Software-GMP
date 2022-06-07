using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; //getConnectionString
using GMP.Researchs.Infrastructure.EFContext;
using System;
using Azure.Storage.Blobs; //blobs storage
using MediatR;
using GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch;
using System.Reflection;

namespace GMP.Researchs.Infrastructure.Startup
{
    public class ResearchsStartup
    {
        public static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);
            ConfigureIOC(services);
            ConfigureMediator(services);
            ConfigureMapper(services);
            ConfigurationBlobStorage(services, configuration);
        }

        private static void ConfigureMapper(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void ConfigureMediator(IServiceCollection services)
        {
            services.AddMediatR(typeof(RegisterStudyResearchCommand).GetTypeInfo().Assembly);
        }

        private static void ConfigureIOC(IServiceCollection services)
        {
            InyectionContainer.Inyection(services);
        }

        private static void ConfigureContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ResearchsDBContext>(options =>
            options.UseSqlServer(
              configuration.GetConnectionString("GMPConnectionString")));
        }

        private static void ConfigurationBlobStorage(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection(key: "AzureBlobStorageConnectionString").Value;
            services.AddSingleton(x => new BlobServiceClient(connectionString: connectionString));
        }
    }
}
