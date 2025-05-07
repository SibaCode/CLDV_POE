using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Create Event
        public IActionResult Create()
        {
            // Fetch venues to populate the dropdown list
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // POST: Create Event
       // POST: Create Event (AJAX Version)
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([FromForm] Event newEvent)
{
    if (ModelState.IsValid)
    {
        // Validate the venue selection
        var venue = await _context.Venues.FindAsync(newEvent.VenueId);
        if (venue == null)
        {
            ModelState.AddModelError("VenueId", "Selected venue is not valid.");
            return Json(new { success = false, message = "Selected venue is not valid." });
        }

        // Add the event to the database
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        // Return success response
        return Json(new { success = true, message = "Event created successfully!" });
    }

    // If the model is invalid, return error messages
    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
    return Json(new { success = false, message = string.Join(", ", errors) });
}

        // Index: Display list of events
        public IActionResult Index()
        {
            return View(_context.Events.ToList());
        }
    }
}
