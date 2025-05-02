using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int VenueId { get; set; }

    [Required]
    public int EventId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }

    // Navigation
    public Venue Venue { get; set; }
    public Event Event { get; set; }
}
