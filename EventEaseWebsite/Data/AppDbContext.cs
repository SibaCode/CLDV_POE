using Microsoft.EntityFrameworkCore;
using EventEase.Models;

namespace EventEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
           protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
       modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany()  // No navigation property in Venue for Events
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

            // Configure the Event-Booking relationship (One-to-Many)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany()  // No navigation property in Event for Bookings
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete

            // Configure the Venue-Booking relationship (One-to-Many)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany()  // No navigation property in Venue for Bookings
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict); // Avoids cascading delete
    }
    }
}

