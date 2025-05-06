
using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly AppDbContext _context;

        public VenuesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Venues
            [Route("Venues")]
            [Route("Venues/Index")]

        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venues.ToListAsync();
            return View(venues);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Venues()
        {
            return RedirectToAction("Index", "Venues", new { area = "" });

        }
        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
 
        public async Task<IActionResult> Create([Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync(); // ✅ This line saves the data to the database
                return RedirectToAction(nameof(Index)); // ✅ Redirects after saving
            }
            return View(venue); // Returns the view with validation errors, if any
        }

      // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Details/1
[HttpGet]
public async Task<IActionResult> Details(int? id)
{
    if (id == null || _context.Venues == null)
    {
        return NotFound();
    }

    var venue = await _context.Venues
        .FirstOrDefaultAsync(m => m.VenueId == id);
    if (venue == null)
    {
        return NotFound();
    }

    return View(venue);
}

    }
}
