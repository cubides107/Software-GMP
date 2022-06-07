using GMP.Researchs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GMP.Researchs.Infrastructure.EFContext
{
    public class ResearchsDBContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<MapHome> MapHomes { get; set; }

        public DbSet<Solicitation> Solicitations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TipoSolicitudEnum> TiposSolicitudes { get; set; }

        public ResearchsDBContext(DbContextOptions<ResearchsDBContext> options) : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de esquema de este modulo
            modelBuilder.HasDefaultSchema("researchs");

            //configurar manualmnete la relacion entre la solicitud y un usuario que crea las solicitudes
            modelBuilder.Entity<Solicitation>()
            .HasOne(p => p.UserCreatesSolicitation)
            .WithMany(b => b.CreatedSolicitations);

            //configurar manualmnete la relacion entre la solicitud y un usuario que GESTIONA las solicitudes
            modelBuilder.Entity<Solicitation>()
            .HasOne(p => p.UserManagesSolicitation)
            .WithMany(b => b.ManagedSolicitations);
        }

    }
}
