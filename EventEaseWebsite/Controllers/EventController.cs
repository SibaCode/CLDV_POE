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
public async Task<IActionResult> Index()
{
    var events = await _context.Events
        .Include(e => e.Venue)
        .ToListAsync();
    return View(events);
}

    // GET: Event/Create
    public IActionResult Create()
    {
        ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName");
        return View();
    }

    // POST: Event/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,Description,VenueId")] Event @event)
    {
        if (ModelState.IsValid)
        {
            _context.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
        return View(@event);
    }
}
}
