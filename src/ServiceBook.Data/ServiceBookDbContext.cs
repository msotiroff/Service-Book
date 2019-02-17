using Microsoft.EntityFrameworkCore;
using ServiceBook.Config;
using ServiceBook.Models.DatabaseModels;

namespace ServiceBook.Data
{
    public class ServiceBookDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<ServiceIntervention> ServiceInterventions { get; set; }

        public DbSet<ServiceItem> ServiceItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseSqlite(DbConstants.SQLiteConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity
                    .HasMany(u => u.Vehicles)
                    .WithOne(v => v.Owner)
                    .HasForeignKey(v => v.OwnerId);
            });

            builder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity
                    .HasMany(v => v.ServiceInterventions)
                    .WithOne(si => si.Vehicle)
                    .HasForeignKey(si => si.VehicleId);
            });

            builder.Entity<ServiceIntervention>(entity =>
            {
                entity.HasKey(si => si.Id);
                entity
                    .HasMany(si => si.ServiceItems)
                    .WithOne(sit => sit.ServiceIntervention)
                    .HasForeignKey(sit => sit.ServiceInterventionId);
            });

            base.OnModelCreating(builder);
        }
    }
}
