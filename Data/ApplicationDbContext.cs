using landlord_be.Models;
using Microsoft.EntityFrameworkCore;

namespace landlord_be.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<ImageLink> ImageLinks => Set<ImageLink>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Personal> Personals => Set<Personal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasOne(p => p.User).WithMany(u => u.Properties).HasForeignKey(p => p.OwnerId);
            modelBuilder.Entity<User>().HasOne(u => u.Personal).HasForeignKey<User>(u => u.PersonalId);
            modelBuilder.Entity<Property>().HasOne(p => p.Address).WithOne(p => a.Property).HasForeignKey<Property>(p => p.AddressId);
            modelBuilder.Entity<ImageLink>().HasOne(i => i.Property).WithMany(p => p.ImageLinks).HasForeignKey(i => i.PropertyId);
        }
    }
}
