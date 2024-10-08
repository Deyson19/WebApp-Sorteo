using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp_Sorteo.Models;
using WebApp_Sorteo.Models.Helpers;

namespace WebApp_Sorteo.Controllers
{
    //[Authorize(Roles = Roles.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult Privacy()
        {
            _logger.LogError($"Acceso denegado para {User.Identity!.Name}, no es admin.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
