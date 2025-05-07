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
        public async Task<IActionResult> Index()
                {
                    var bookings = _context.Bookings
                    .Include(b => b.Event)
                    .Include(b => b.Venue)
                    .OrderByDescending(b => b.BookingDate);

                return View(await bookings.ToListAsync());
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
    // Check if the venue is already booked for the same date
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

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.BookingId == booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VenueList = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewBag.EventList = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            return View(booking);
        }

       public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var venue = await _context.Venues
        .FirstOrDefaultAsync(m => m.VenueId == id);

    if (venue == null)
    {
        return NotFound();
    }

    // Check if there are any active bookings for the venue
    bool hasActiveBookings = await _context.Bookings
        .AnyAsync(b => b.VenueId == venue.VenueId);

    if (hasActiveBookings)
    {
        // Add an error or alert here
        TempData["ErrorMessage"] = "This venue has active bookings and cannot be deleted.";
        return RedirectToAction(nameof(Index));
    }

    _context.Venues.Remove(venue);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

        // POST: Booking/Delete/5
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
