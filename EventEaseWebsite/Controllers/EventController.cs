using EventEase.Data;
using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IConfiguration _configuration;
        public EventController(AppDbContext context , IBlobService blobService , IConfiguration configuration)
        {
            _context = context;
            _blobService = blobService;
            _configuration = configuration;
        }
public async Task<IActionResult> Index(string searchString)
{
    var eventsQuery = _context.Events.Include(e => e.Venue).AsQueryable();

    if (!string.IsNullOrEmpty(searchString))
    {
        eventsQuery = eventsQuery.Where(e =>
            e.EventName.Contains(searchString) ||
            e.Description.Contains(searchString) ||
            e.Venue.VenueName.Contains(searchString)
        );
    }

    var events = await eventsQuery.ToListAsync();

    var bookedEventIds = _context.Bookings
                                 .Select(b => b.EventId)
                                 .Distinct()
                                 .ToHashSet();

    ViewBag.BookedEventIds = bookedEventIds;
    ViewBag.CurrentFilter = searchString;

    return View(events);
}


        // GET: Event/Create
        public IActionResult Create()
        {
            ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName");
            ViewBag.EventTypeList = new SelectList(_context.EventType, "EventTypeId", "Name");
            return View();
        }

       [HttpPost]
[ValidateAntiForgeryToken]

public async Task<IActionResult> Create(Event @event, IFormFile imageFile)
{
    try
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imageUrl = await _blobService.UploadFileAsync(imageFile, "event-images");
                @event.ImageUrl = imageUrl;
            }

            _context.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    catch (Exception ex)
    {
        return Content($"Error: {ex.Message}");
    }

    ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
    ViewBag.EventTypeList = new SelectList(_context.EventType, "EventTypeId", "Name", @event.EventTypeId);

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
        ViewBag.EventTypeList = new SelectList(_context.EventType, "EventTypeId", "Name", @event.EventTypeId);

        return View(@event);
    }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId,ImageUrl")] Event @event, IFormFile imageFile)
{
    if (id != @event.EventId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // If an image file is provided, upload it and update the ImageUrl
            if (imageFile != null && imageFile.Length > 0)
            {
                // Upload the new image and get the new URL
                var imageUrl = await _blobService.UploadFileAsync(imageFile, "event-images");
                @event.ImageUrl = imageUrl; // Update the ImageUrl property
            }
            // If no image is uploaded, retain the existing ImageUrl (don't modify it)
            
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

    // Populate the VenueList dropdown
    ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
    ViewBag.EventTypeList = new SelectList(_context.EventType, "EventTypeId", "Name", @event.EventTypeId);

    return View(@event);
}
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
