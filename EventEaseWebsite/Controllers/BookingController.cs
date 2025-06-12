using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }
    public async Task<IActionResult> Index(
    string searchString,
    int? eventTypeId,
    int? venueId,
    DateTime? startDate,
    DateTime? endDate)
{
    // Start query
    var bookingsQuery = _context.Bookings
        .Include(b => b.Event).ThenInclude(e => e.EventType)
        .Include(b => b.Venue)
        .AsQueryable();

    // Apply filters
    if (!string.IsNullOrEmpty(searchString))
    {
        bookingsQuery = bookingsQuery.Where(b =>
            b.Event.EventName.Contains(searchString) ||
            b.Venue.VenueName.Contains(searchString) ||
            b.BookingDate.ToString().Contains(searchString));
    }

    if (eventTypeId.HasValue)
    {
        bookingsQuery = bookingsQuery.Where(b => b.Event.EventTypeId == eventTypeId);
    }

    if (venueId.HasValue)
    {
        bookingsQuery = bookingsQuery.Where(b => b.VenueId == venueId);
    }

    if (startDate.HasValue)
    {
        bookingsQuery = bookingsQuery.Where(b => b.BookingDate >= startDate.Value);
    }

    if (endDate.HasValue)
    {
        bookingsQuery = bookingsQuery.Where(b => b.BookingDate <= endDate.Value);
    }

    // Populate ViewBags for form controls
    ViewBag.EventTypes = new SelectList(await _context.EventType.ToListAsync(), "EventTypeId", "Name", eventTypeId);
    ViewBag.Venues = new SelectList(await _context.Venues.ToListAsync(), "VenueId", "VenueName", venueId);
    
    ViewBag.CurrentFilter = searchString;
    ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
    ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

    var bookings = await bookingsQuery
        .OrderByDescending(b => b.BookingDate)
        .ToListAsync();

    return View(bookings);
}



        public IActionResult Create()
        {
            ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName");
            ViewBag.EventList = new SelectList(_context.Events, "EventId", "EventName");
            return View();
        }

        // In BookingController - POST for Create or Edit
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("BookingId, EventId, VenueId, BookingDate")] Booking booking)
{
    // Check for duplicate bookings
    bool isAlreadyBooked = await _context.Bookings
        .AnyAsync(b => b.VenueId == booking.VenueId 
                       && b.BookingDate.Date == booking.BookingDate.Date);

    if (isAlreadyBooked)
    {
        ModelState.AddModelError("BookingDate", "The selected venue is already booked for this date.");
    }

    if (ModelState.IsValid)
    {
        _context.Add(booking);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Repopulate dropdowns
    ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
    ViewBag.EventList = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);

    return View(booking);
}


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewBag.EventList = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId, EventId, VenueId, BookingDate")] Booking booking)
        {
            if (id != booking.BookingId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.BookingId == booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Repopulate dropdowns if validation fails
            ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewBag.EventList = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            
            return View(booking);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

       public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }



       [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
