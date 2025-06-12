using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
public class EventType
{
    public int EventTypeId { get; set; }
    public string Name { get; set; }

    // Navigation
    public ICollection<Event> Events { get; set; }
}


}
