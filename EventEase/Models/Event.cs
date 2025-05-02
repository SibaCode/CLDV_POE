using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Event
{
    public int Id { get; set; }

    [Required]
    public string EventName { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    public string Description { get; set; }

    public ICollection<Booking> Bookings { get; set; }
}
