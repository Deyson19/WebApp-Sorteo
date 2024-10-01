using WebApp_Sorteo.Models;

namespace WebApp_Sorteo.Services.Contract
{
    public interface IMercadoPagoService
    {
        Task<string> CrearPago(EnviarPago enviarPago);
        Task<string> CrearPagoPaypal(EnviarPago enviarPago, string? baseUrl);
    }
}