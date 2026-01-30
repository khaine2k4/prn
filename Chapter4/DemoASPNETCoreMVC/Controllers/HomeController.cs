using Microsoft.AspNetCore.Mvc;
using DemoASPNETCoreMVC.Models;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        HomeModel message = new HomeModel();
        return View(message);
    }
}
