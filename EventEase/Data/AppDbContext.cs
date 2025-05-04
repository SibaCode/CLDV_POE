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
                public DbSet<TestModel> TestModels { get; set; }  // New table for the TestModel


       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Venue -> Events
    // modelBuilder.Entity<Event>()
    //     .HasOne(e => e.Venue)
    //     .WithMany(v => v.Events)
    //     .HasForeignKey(e => e.VenueId)
    //     .OnDelete(DeleteBehavior.Restrict); // no cascade

    // // Venue -> Bookings
    // modelBuilder.Entity<Booking>()
    //     .HasOne(b => b.Venue)
    //     .WithMany(v => v.Bookings)
    //     .HasForeignKey(b => b.VenueId)
    //     .OnDelete(DeleteBehavior.Restrict); // also restrict to avoid conflict

    // Event -> Bookings
    modelBuilder.Entity<Booking>()
        .HasOne(b => b.Event)
        .WithMany(e => e.Bookings)
        .HasForeignKey(b => b.EventId)
        .OnDelete(DeleteBehavior.Restrict); // optional: pick behavior
}

    }
}
