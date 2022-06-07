using GMP.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Infrastructure.EFContext
{
    public class UsersDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDBContext(DbContextOptions<UsersDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de esquema de este modulo
            modelBuilder.HasDefaultSchema("users");
        }
    }
}
