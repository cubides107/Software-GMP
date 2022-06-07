using GMP.Domain;
using GMP.Infrastructure;
using GMP.Infrastructure.Exceptions;
using GMP.Researchs.Domain.Ports;
using GMP.Researchs.Infrastructure.EFContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.Infrastructure.Repositories
{
    public class ResearchsRepositorySQL : Repository, IResearchsRepository
    {
        public ResearchsRepositorySQL(ResearchsDBContext context): base(context)
        {

        }
    }
}
