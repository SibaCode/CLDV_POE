
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventEase.Models
{
public class EventCreateViewModel
{
    public Event Event { get; set; }
    public SelectList Venues { get; set; }
}

}

