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
            var events = await _context.Events.ToListAsync();

            // Find all EventIds that are linked to Bookings
            var bookedEventIds = _context.Bookings
                                        .Select(b => b.EventId)
                                        .Distinct()
                                        .ToHashSet();

            ViewBag.BookedEventIds = bookedEventIds;

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

// GET: Event/Edit/5
  public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        // Populating Venue dropdown list for editing
        ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
        return View(@event);
    }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId")] Event @event)
{
    if (id != @event.EventId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(@event);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Events.Any(e => e.EventId == @event.EventId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }

    // Ensure the VenueList is set for the dropdown
    ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
    return View(@event);
}

// GET: Event/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var eventItem = await _context.Events
        .FirstOrDefaultAsync(m => m.EventId == id);

    if (eventItem == null)
    {
        return NotFound();
    }

    // Check if there are any active bookings for the event
    bool hasActiveBookings = await _context.Bookings
        .AnyAsync(b => b.EventId == eventItem.EventId);

    if (hasActiveBookings)
    {
        TempData["ErrorMessage"] = "This event has active bookings and cannot be deleted.";
        return RedirectToAction(nameof(Index));
    }

    _context.Events.Remove(eventItem);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}


// POST: Event/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var @event = await _context.Events.FindAsync(id);
    if (@event == null)
    {
        return NotFound();
    }

    _context.Events.Remove(@event);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
