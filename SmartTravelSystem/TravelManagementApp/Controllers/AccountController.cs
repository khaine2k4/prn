using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers;

public class AccountController : Controller
{
    private readonly TravelCenterContext _db;

    public AccountController(TravelCenterContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string code, string password)
    {
        var customer = await _db.Customers
            .FirstOrDefaultAsync(c => c.Code == code && c.Password == password);

        if (customer == null)
        {
            ViewBag.Error = "Invalid Code or Password!";
            return View();
        }

        // lưu session
        HttpContext.Session.SetInt32("CustomerID", customer.CustomerId);
        HttpContext.Session.SetString("CustomerCode", customer.Code);
        HttpContext.Session.SetString("CustomerName", customer.FullName);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}