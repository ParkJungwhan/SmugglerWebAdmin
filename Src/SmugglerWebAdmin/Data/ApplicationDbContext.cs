using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmugglerWebAdmin.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionEnvironment> RegionEnvironments { get; set; }
        public DbSet<UserRegionPermission> UserRegionPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RegionEnvironment>()
                .HasOne<Region>()
                .WithMany()
                .HasForeignKey(e => e.RegionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserRegionPermission>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserRegionPermission>()
                .HasOne<RegionEnvironment>()
                .WithMany()
                .HasForeignKey(p => p.RegionEnvironmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserRegionPermission>()
                .HasIndex(p => new { p.UserId, p.RegionEnvironmentId })
                .IsUnique();
        }
    }
}
