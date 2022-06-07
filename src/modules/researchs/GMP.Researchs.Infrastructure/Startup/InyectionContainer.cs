using GMP.Researchs.Domain.Factories;
using GMP.Researchs.Domain.Ports;
using GMP.Researchs.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Infrastructure.Startup
{
    public class InyectionContainer
    {
        public static void Inyection(IServiceCollection services)
        {
            services.AddScoped<IResearchsRepository, ResearchsRepositorySQL>();
            services.AddScoped<IResearchsRepositoryBlob, ResearchsRepositoryBlob>();
            services.AddScoped<IResearchsFactory, ResearchsFactory>();
        }
    }
}
