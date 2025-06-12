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
        public DbSet<EventType> EventType { get; set; }

           protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
       modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany()  
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany() 
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany() 
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict); 

    }
    }
}

