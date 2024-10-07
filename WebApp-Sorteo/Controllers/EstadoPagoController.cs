using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp_Sorteo.Data;
using WebApp_Sorteo.Helpers;
using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Controllers
{
    [Authorize]
    public class EstadoPagoController : Controller
    {
        private readonly ILogger<EstadoPagoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public EstadoPagoController(ILogger<EstadoPagoController> logger, ApplicationDbContext context, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Index() => View();
        
        public async Task<IActionResult> Success([FromQuery] PaymentResponse paymentResponse)
        {
            if (ModelState.IsValid)
            {
                var pago = await _context.Pagos.FirstOrDefaultAsync(x=>x.PrefId == paymentResponse.payment_id);
                if (pago == null)
                {
                    TempData[DS.Error] = "El pago no existe";
                    return RedirectToAction("Index","Home");
                }
                pago.MetodoPago = paymentResponse.payment_type;
                pago.EstadoPago = EstadoPago.Pagado;
                pago.FechaPago = DateTime.Now;
                pago.DetallesPago = "Pago realizado correctamente";

                var ticket = await _context.Tickets.Include(c=>c.Usuario).Include(x=>x.Sorteo).FirstOrDefaultAsync(x => x.Id == pago.TicketId);
                if (ticket == null)
                {
                    TempData[DS.Error] = "No existe un ticket para ese pago";
                    return RedirectToAction("Index","Home");
                }
                ticket.EstadoPago = EstadoPago.Pagado;
                ticket.FechaCompra = DateTime.Now;
                ticket.EstadoTicket = EstadoTicket.Inactivo;
                ticket.MontoPagado = pago.Monto;

                await _context.SaveChangesAsync();
                var cuerpoCorreo = $"Se ha pagado el ticket: {ticket.NumeroTicket} y " +
                    $"ya estas participando en el sorteo de: {ticket.Sorteo!.Descripcion}, Mucha suerte";

                await _emailSender.SendEmailAsync(ticket.Usuario!.Email!,"Haz realizado un pago.",cuerpoCorreo);
            }
            return Json(paymentResponse);
        }
        public async Task<IActionResult> Failure([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }
        public async Task<IActionResult> Pending([FromQuery] PaymentResponse paymentResponse)
        {
            return Json(paymentResponse);
        }
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