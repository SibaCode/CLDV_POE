using EventEase.Data;
using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models; // ⬅️ Required for BlobHttpHeaders


namespace EventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IConfiguration _configuration;

        public VenueController(AppDbContext context , IBlobService blobService , IConfiguration configuration) 
        {
            _context = context;
            _blobService = blobService;
            _configuration = configuration;
        }private bool VenueExists(int id)
{
    return _context.Venues.Any(e => e.VenueId == id);
}


        public async Task<IActionResult> Index(string searchString)
            {
                var venuesQuery = _context.Venues.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    venuesQuery = venuesQuery.Where(v =>
                        v.VenueName.Contains(searchString) ||
                        v.Location.Contains(searchString) ||
                        v.Capacity.ToString().Contains(searchString)
                    );
                }

                var venues = await venuesQuery.ToListAsync();

                // Get booked VenueIds
                var bookedVenueIds = _context.Bookings
                                            .Select(b => b.VenueId)
                                            .Distinct()
                                            .ToHashSet();

                ViewBag.BookedVenueIds = bookedVenueIds;
                ViewBag.CurrentFilter = searchString;

                return View(venues);
            }


        
public IActionResult Create() => View();

// POST
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Venue venue, IFormFile imageFile)
{
    if (ModelState.IsValid)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            // var containerName = _configuration.GetSection("AzureBlobStorage")["ContainerName"];
            venue.ImageUrl = await _blobService.UploadFileAsync(imageFile, "eventeaseimages");
        }

        _context.Add(venue);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    return View(venue);
}
      

        // GET: Venue/Edit/5
      public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity")] Venue venue , IFormFile imageFile)
        {
           if (id != venue.VenueId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Upload the new image and get the URL
                var imageUrl = await _blobService.UploadFileAsync(imageFile, "venue-images");

                // Optionally, delete the old image if you want
                if (!string.IsNullOrEmpty(venue.ImageUrl))
                {
                    var oldImageName = Path.GetFileName(new Uri(venue.ImageUrl).LocalPath);
                    await _blobService.DeleteFileAsync(oldImageName, "venue-images");
                }

                venue.ImageUrl = imageUrl; // Set the new image URL
            }

            _context.Update(venue);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VenueExists(venue.VenueId))
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
    return View(venue);
        }
[Route("Venue/Image/{fileName}")]
public async Task<IActionResult> GetImage(string fileName)
{
    var containerName = "eventeaseimages";
    var containerClient = new BlobContainerClient(_configuration.GetConnectionString("AzureBlobStorage"), containerName);
    var blobClient = containerClient.GetBlobClient(fileName);

    if (!await blobClient.ExistsAsync())
    {
        return NotFound();
    }

    var blobDownloadInfo = await blobClient.DownloadAsync();
    return File(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
}

        // GET: Venue/Delete/5
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

            return View(venue);
        }

        // POST: Venue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
