
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class CreateEventDto
{
    public string EventName { get; set; }
    public DateTime EventDate { get; set; }
    public string Description { get; set; }
    public int VenueId { get; set; }
}
}

