using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
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

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues
        public IActionResult Index()
        {
            var venues = _context.Venues.ToList();
            return View(venues);
        }

        // GET: Venues/Edit/5
[HttpGet("Edit/{id}")]
public async Task<IActionResult> Edit(int id)
{
    var venue = await _context.Venues.FindAsync(id);
    if (venue == null)
    {
        return NotFound();
    }
    return View(venue);
}

// POST: Venues/Edit/5
[HttpPost("Edit/{id}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Venue venue)
{
    if (id != venue.VenueId)
        return NotFound();

    if (ModelState.IsValid)
    {
        _context.Update(venue);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(venue);
}

// GET: Venues/Delete/5
[HttpGet("Delete/{id}")]
public async Task<IActionResult> Delete(int id)
{
    var venue = await _context.Venues.FindAsync(id);
    if (venue == null)
    {
        return NotFound();
    }
    return View(venue);
}

// POST: Venues/Delete/5
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


    }
}
