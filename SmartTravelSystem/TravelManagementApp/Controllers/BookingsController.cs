using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers;

public class BookingsController : Controller
{
    private readonly TravelCenterContext _db;
    public BookingsController(TravelCenterContext db) => _db = db;

    private int? CustomerId => HttpContext.Session.GetInt32("CustomerID");

    public async Task<IActionResult> MyBookings(string filter = "Pending")
    {
        if (CustomerId == null) return RedirectToAction("Login", "Account");

        var query = _db.Bookings
            .Include(b => b.Trip)
            .Where(b => b.CustomerId == CustomerId.Value);

        // Filter pending
        if (filter == "Pending")
            query = query.Where(b => b.Status == "Pending");

        var list = await query
            .OrderBy(b => b.BookingDate)   // sort asc
            .ToListAsync();

        ViewBag.Filter = filter;
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (CustomerId == null) return RedirectToAction("Login", "Account");

        // chọn trip Available
        ViewBag.Trips = await _db.Trips.Where(t => t.Status == "Available").ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(int tripId)
    {
        if (CustomerId == null) return RedirectToAction("Login", "Account");

        var booking = new Booking
        {
            TripId = tripId,
            CustomerId = CustomerId.Value,
            BookingDate = DateOnly.FromDateTime(DateTime.Now),
            Status = "Pending"
        };

        _db.Bookings.Add(booking);

        // Option: set trip to Booked (nếu muốn)
        var trip = await _db.Trips.FindAsync(tripId);
        if (trip != null) trip.Status = "Booked";

        await _db.SaveChangesAsync();
        return RedirectToAction("MyBookings");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int bookingId, string status)
    {
        if (CustomerId == null) return RedirectToAction("Login", "Account");

        var booking = await _db.Bookings.FindAsync(bookingId);
        if (booking == null) return NotFound();

        // chỉ cho update booking của chính mình
        if (booking.CustomerId != CustomerId.Value) return Forbid();

        booking.Status = status;
        await _db.SaveChangesAsync();
        return RedirectToAction("MyBookings");
    }
}