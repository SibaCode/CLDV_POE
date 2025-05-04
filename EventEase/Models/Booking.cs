using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
   public class Booking
{
    public int BookingId { get; set; }

    public int EventId { get; set; } // FK to Event
    public Event Event { get; set; }

    public int VenueId { get; set; } // FK to Venue
    public Venue Venue { get; set; }

    public DateTime BookingDate { get; set; }
}

}
