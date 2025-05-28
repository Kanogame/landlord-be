using landlord_be.Models;
using Microsoft.EntityFrameworkCore;

namespace landlord_be.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // property
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<PropertyAttribute> Attributes => Set<PropertyAttribute>();
        public DbSet<ImageLink> ImageLinks => Set<ImageLink>();
        public DbSet<Address> Addresses => Set<Address>();

        // user
        public DbSet<User> Users => Set<User>();
        public DbSet<Personal> Personals => Set<Personal>();
        public DbSet<VerificationPending> VerificationPendings => Set<VerificationPending>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // property
            modelBuilder
                .Entity<Property>()
                .HasOne(p => p.User)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.OwnerId);
            modelBuilder
                .Entity<Property>()
                .HasOne(p => p.Address)
                .WithOne(a => a.Property)
                .HasForeignKey<Property>(p => p.AddressId);
            modelBuilder
                .Entity<Property>()
                .HasMany(p => p.ImageLinks)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyId);
            modelBuilder
                .Entity<Property>()
                .HasMany(p => p.PropertyAttributes)
                .WithOne(a => a.Property)
                .HasForeignKey(a => a.PropertyId);

            // user
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.Personal)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PersonalId);
        }
    }
}
