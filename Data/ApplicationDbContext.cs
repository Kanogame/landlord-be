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
        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();

        // chat
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

        // calendar
        public DbSet<CalendarPeriod> CalendarPeriods => Set<CalendarPeriod>();

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

            // bookmark configurations
            modelBuilder
                .Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Bookmark>()
                .HasOne(b => b.Property)
                .WithMany()
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Create unique constraint to prevent duplicate bookmarks
            modelBuilder
                .Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.PropertyId })
                .IsUnique();

            modelBuilder
                .Entity<CalendarPeriod>()
                .HasOne(cp => cp.Property)
                .WithMany()
                .HasForeignKey(cp => cp.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<CalendarPeriod>()
                .HasOne(cp => cp.AttachedUser)
                .WithMany()
                .HasForeignKey(cp => cp.AttachedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Index for better query performance
            modelBuilder
                .Entity<CalendarPeriod>()
                .HasIndex(cp => new
                {
                    cp.PropertyId,
                    cp.StartDate,
                    cp.EndDate,
                });
        }
    }
}
