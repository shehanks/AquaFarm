using AquaFarm.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AquaFarm.Infrastructure.Data
{
    public class AquaFarmDbContext : DbContext
    {
        public DbSet<FishFarm> FishFarms { get; set; }

        public DbSet<Worker> Workers { get; set; }

        public AquaFarmDbContext(DbContextOptions<AquaFarmDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FishFarm>(entity =>
            {
                entity.Property(f => f.Name).HasMaxLength(200).IsRequired();
                entity.Property(f => f.Picture).HasMaxLength(500);

                entity.Property(f => f.GPSLatitude).HasPrecision(9, 4).IsRequired();
                entity.Property(f => f.GPSLongitude).HasPrecision(9, 4).IsRequired();
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.Property(w => w.Name).HasMaxLength(200).IsRequired();
                entity.Property(w => w.Email).HasMaxLength(200).IsRequired();
                entity.Property(w => w.Picture).HasMaxLength(500);

                entity.Property(w => w.Position)
                      .HasConversion<string>()
                      .HasMaxLength(50)
                      .IsRequired();
            });
        }
    }
}
