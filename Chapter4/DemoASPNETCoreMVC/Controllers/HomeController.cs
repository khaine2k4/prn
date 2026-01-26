using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        HomeModel message = new HomeModel();
        return View(message);
    }
}
