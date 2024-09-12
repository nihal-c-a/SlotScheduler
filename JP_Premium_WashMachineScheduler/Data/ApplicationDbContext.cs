using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JP_Premium_WashMachineScheduler.Models;

namespace JP_Premium_WashMachineScheduler.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<WashingMachine> WashingMachines { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuring the relationship between Tenant and IdentityUser
            builder.Entity<Tenant>()
                .HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<Tenant>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the relationship between Booking and Tenant
            builder.Entity<Booking>()
                .HasOne(b => b.Tenant)
                .WithMany()
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the relationship between Booking and WashingMachine
            builder.Entity<Booking>()
                .HasOne(b => b.WashingMachine)
                .WithMany()
                .HasForeignKey(b => b.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Additional configurations if necessary
        }
    }
}
