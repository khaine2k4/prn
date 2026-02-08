using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers;

public class TripsController : Controller
{
    private readonly TravelCenterContext _db;
    public TripsController(TravelCenterContext db) => _db = db;

    private bool IsLoggedIn() => HttpContext.Session.GetInt32("CustomerID") != null;

    public async Task<IActionResult> Index()
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
        var trips = await _db.Trips.ToListAsync();
        return View(trips);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Trip trip)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid) return View(trip);

        _db.Trips.Add(trip);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        var trip = await _db.Trips.FindAsync(id);
        if (trip == null) return NotFound();
        return View(trip);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Trip trip)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        _db.Trips.Update(trip);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
