using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Controllers
{
    [Authorize]
    public class EstadoPagoController : Controller
    {
        private readonly ILogger<EstadoPagoController> _logger;

        public EstadoPagoController(ILogger<EstadoPagoController> logger)
        {
            _logger = logger;
        }
        [HttpGet("Success")]
        public async Task<IActionResult> Success([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }
        [HttpGet("Failure")]
        public async Task<IActionResult> Failure([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }
        [HttpGet("Pending")]
        public async Task<IActionResult> Pending([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> Notification([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}