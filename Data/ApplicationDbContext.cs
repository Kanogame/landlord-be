using landlord_be.Models;
using Microsoft.EntityFrameworkCore;

namespace landlord_be.Data {
    public class ApplicationDbContext: DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}

        public DbSet<User> Users {get; set;}
        public DbSet<Property> Properties {get; set;}
        public DbSet<ImageLink> ImageLinks {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasOne(p => p.User).WithMany(u => u.Properties).HasForeignKey(p => p.OwnerId);
        }
    }
}