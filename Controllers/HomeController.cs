
using Microsoft.AspNetCore.Mvc;

namespace BeFit.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Privacy() => View();
}
