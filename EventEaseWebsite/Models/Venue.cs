
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{public class Venue
{
    public int VenueId { get; set; }
    public string VenueName { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public ICollection<Event> Events { get; set; }
}
}