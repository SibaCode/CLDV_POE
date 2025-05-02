using Microsoft.EntityFrameworkCore;
namespace EventEase.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

          // Setting up one-to-many relationship between Venue and Booking
    modelBuilder.Entity<Booking>()
        .HasOne(b => b.Venue)
        .WithMany(v => v.Bookings)
        .HasForeignKey(b => b.VenueId)
        .OnDelete(DeleteBehavior.Cascade);  // Cascade delete if needed

    // Setting up one-to-many relationship between Event and Booking
    modelBuilder.Entity<Booking>()
        .HasOne(b => b.Event)
        .WithMany(e => e.Bookings)
        .HasForeignKey(b => b.EventId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
