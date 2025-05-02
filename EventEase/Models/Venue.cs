using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Venue
{
    public int Id { get; set; }

    [Required]
    public string VenueName { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public int Capacity { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<Booking> Bookings { get; set; }
}
