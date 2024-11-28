using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Controllers
{
     public class HomeController(ILogger<HomeController> logger) : Controller
     {
          private readonly ILogger<HomeController> _logger = logger;

          [ResponseCache(Duration = 100, Location = ResponseCacheLocation.Client, NoStore = false)]
          public IActionResult Index() => View();
          public IActionResult Privacy() => View();

          [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
          public IActionResult Error()
          {
               return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
          }
     }
}
