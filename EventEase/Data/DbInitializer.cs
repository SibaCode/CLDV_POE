using EventEase.Controllers;
using EventEase.Data;
using System;
using System.Linq;

namespace EventEase.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Prevent duplicate seeding
            if (context.Venues.Any() || context.Events.Any() || context.Bookings.Any())
                return;

            // Seed Venues
            var venues = new Venue[]
            {
                new Venue { VenueName = "Grand Hall", Location = "Cape Town", Capacity = 300, ImageUrl = "http://example.com/image1.jpg" },
                new Venue { VenueName = "Beachside Pavilion", Location = "Durban", Capacity = 150, ImageUrl = "http://example.com/image2.jpg" },
                new Venue { VenueName = "Skyline Roof", Location = "Johannesburg", Capacity = 200, ImageUrl = "http://example.com/image3.jpg" }
            };
            context.Venues.AddRange(venues);
            context.SaveChanges();

            // Seed Events
            var events = new Event[]
            {
                new Event { EventName = "Wedding Ceremony", EventDate = DateTime.Now.AddDays(10), Description = "A beautiful wedding ceremony" },
                new Event { EventName = "Tech Conference", EventDate = DateTime.Now.AddDays(15), Description = "A tech conference for innovators" },
                new Event { EventName = "Music Festival", EventDate = DateTime.Now.AddDays(30), Description = "An exciting music festival" }
            };
            context.Events.AddRange(events);
            context.SaveChanges();

            // Seed Bookings (associate with existing Events and Venues)
            var bookings = new Booking[]
            {
                new Booking { BookingDate = DateTime.Now.AddDays(5), EventId = 1, VenueId = 1 },
                new Booking { BookingDate = DateTime.Now.AddDays(10), EventId = 2, VenueId = 2 },
                new Booking { BookingDate = DateTime.Now.AddDays(20), EventId = 3, VenueId = 3 }
            };
            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}
