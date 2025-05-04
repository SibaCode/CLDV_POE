using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventEase.Models
{
    public class Venue
    {
       [Key]
    public int VenueId { get; set; }

    [Required]
    public string VenueName { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public int Capacity { get; set; }

    public string ImageUrl { get; set; }

    // // Navigation properties (do not mark these as required)
    // public ICollection<Booking> Bookings { get; set; }
    // public ICollection<Event> Events { get; set; }
    }
}
