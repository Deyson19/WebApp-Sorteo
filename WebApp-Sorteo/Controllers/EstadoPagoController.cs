using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
                    var pago = await _context.Pagos.FirstOrDefaultAsync(x => x.PrefId == paymentResponse.preference_id);
                    if (pago == null)
                    {
                         TempData[DS.Error] = "El pago no existe";
                         return RedirectToAction("Index", "Home");
                    }
                    pago.MetodoPago = MetodoPago(paymentResponse.payment_type);
                    pago.EstadoPago = EstadoPago.Pagado;
                    pago.FechaPago = DateTime.Now;
                    pago.DetallesPago = "Pago realizado correctamente";

                    _context.Pagos.Update(pago);

                    var ticket = await _context.Tickets.Include(c => c.Usuario).Include(x => x.Sorteo).FirstOrDefaultAsync(x => x.Id == pago.TicketId);
                    if (ticket == null)
                    {
                         TempData[DS.Error] = "No existe un ticket para ese pago";
                         return RedirectToAction("Index", "Home");
                    }
                    ticket.EstadoPago = EstadoPago.Pagado;
                    ticket.FechaCompra = DateTime.Now;
                    ticket.EstadoTicket = EstadoTicket.Inactivo;
                    ticket.MontoPagado = pago.Monto;

                    _context.Tickets.Update(ticket);

                    await _context.SaveChangesAsync();
                    var cuerpoCorreo = $"Se ha pagado el ticket: {ticket.NumeroTicket} y " +
                        $"ya estas participando en el sorteo de: {ticket.Sorteo!.Descripcion}, Mucha suerte";

                    _logger.LogInformation($"Enviar correo: {cuerpoCorreo}");
                    await _emailSender.SendEmailAsync(ticket.Usuario!.Email!, "Haz realizado un pago.", cuerpoCorreo);
               }
               Console.WriteLine("\n \n");
               _logger.LogInformation("Se ha realizado un pago.");
               return Json(paymentResponse);
          }
          public IActionResult Failure([FromQuery] PaymentResponse paymentResponse)
          {
               _logger.LogError(
                    $"El pago no se realizó: {paymentResponse.ToJson()}"
               );
               return Json(paymentResponse);
          }
          public IActionResult Pending([FromQuery] PaymentResponse paymentResponse)
          {
               _logger.LogWarning($"El pago está pendiente de confirmación");
               return Json(paymentResponse);
          }
          public IActionResult Notification([FromQuery] object paymentResponse)
          {
               _logger.LogInformation($"Se ha recibido una notificación de pago: {paymentResponse.ToJson()}");
               return Json(paymentResponse);
          }


          [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
          public IActionResult Error()
          {
               return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
          }

          private string MetodoPago(string paymentType)
          {
               string _metodoPago;
               //*Mercado pago payment types
               switch (paymentType)
               {
                    case "debit_card":
                         _metodoPago = "Tarjeta Débito";
                         break;
                    case "credit_card":
                         _metodoPago = "Tarjeta de Crédito";
                         break;
                    case "bank_transfer":
                         _metodoPago = "Transferencia Bancaria";
                         break;
                    case "cash":
                         _metodoPago = "Efectivo";
                         break;
                    default:
                         _metodoPago = "Otro";
                         break;
               }
               return _metodoPago;
          }
     }
}