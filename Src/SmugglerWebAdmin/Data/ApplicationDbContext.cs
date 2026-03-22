using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmugglerWebAdmin.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceRegion> ServiceRegions { get; set; }
        public DbSet<RegionEnvironment> RegionEnvironments { get; set; }
        public DbSet<UserServicePermission> UserServicePermissions { get; set; }
        public DbSet<UserServiceRegionPermission> UserServiceRegionPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // L2: ServiceRegion → Service
            builder.Entity<ServiceRegion>()
                .HasOne<Service>()
                .WithMany()
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // L3: RegionEnvironment → ServiceRegion
            builder.Entity<RegionEnvironment>()
                .HasOne<ServiceRegion>()
                .WithMany()
                .HasForeignKey(e => e.ServiceRegionId)
                .OnDelete(DeleteBehavior.Cascade);

            // L1 권한: (UserId, ServiceId) Unique
            builder.Entity<UserServicePermission>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserServicePermission>()
                .HasOne<Service>()
                .WithMany()
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserServicePermission>()
                .HasIndex(p => new { p.UserId, p.ServiceId })
                .IsUnique();

            // L2 권한: (UserId, ServiceRegionId) Unique
            builder.Entity<UserServiceRegionPermission>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserServiceRegionPermission>()
                .HasOne<ServiceRegion>()
                .WithMany()
                .HasForeignKey(p => p.ServiceRegionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserServiceRegionPermission>()
                .HasIndex(p => new { p.UserId, p.ServiceRegionId })
                .IsUnique();
        }
    }
}
