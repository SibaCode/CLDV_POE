using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            // Populate Venue dropdown list for Event creation
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,VenueId,Description")] Event eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If something goes wrong, pass the venue data back to the view
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventModel.VenueId);
            return View(eventModel);
        }

        // GET: Event/Index
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Venue) // Include the Venue details with the event
                .ToListAsync();
            return View(events);
        }
    }
}
