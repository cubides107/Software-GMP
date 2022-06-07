using GMP.Domain;
using GMP.Infrastructure;
using GMP.Infrastructure.Exceptions;
using GMP.Users.Domain.Ports;
using GMP.Users.Infrastructure.EFContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Infrastructure.Repositories
{
    public class UsersRepositorySQL : Repository, IUsersRepositories
    {
        public UsersRepositorySQL(UsersDBContext context) : base(context)
        {

        }
    }
}
