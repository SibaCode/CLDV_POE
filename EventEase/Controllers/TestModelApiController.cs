using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class TestModelController : Controller
    {
        private readonly AppDbContext _context;

        public TestModelController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TestModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestModel testModel)
        {
            if (ModelState.IsValid)
            {
                _context.TestModels.Add(testModel); // Add the new TestModel to the DB
                await _context.SaveChangesAsync(); // Save to DB
                return RedirectToAction(nameof(Index)); // Redirect to a list page (optional)
            }

            return View(testModel); // Return the view with the entered data if validation fails
        }

        // Optionally: A list page to view created entries
        public async Task<IActionResult> Index()
        {
            var testModels = await _context.TestModels.ToListAsync();
            return View(testModels);
        }
    }
}
