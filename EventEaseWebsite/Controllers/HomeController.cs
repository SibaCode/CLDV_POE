using EventEase.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

namespace EventEase.Controllers
{

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        ViewBag.TotalVenues = _context.Venues.Count();
        ViewBag.TotalEvents = _context.Events.Count();
        ViewBag.TotalBookings = _context.Bookings.Count();

        return View();
    }
}


  }
