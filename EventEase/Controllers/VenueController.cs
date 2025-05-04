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
    }
}
