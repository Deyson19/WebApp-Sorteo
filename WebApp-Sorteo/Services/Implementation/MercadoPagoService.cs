using WebApp_Sorteo.Models;
using WebApp_Sorteo.Services.Contract;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using MercadoPago.Config;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Core;
using PayPalHttp;
using WebApp_Sorteo.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApp_Sorteo.Services.Implementation
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly string AccessToken;
        private readonly string PublicKey;
        private readonly string clientId;
        private readonly string secretId;

        public MercadoPagoService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
            AccessToken = _configuration.GetValue<string>("MercadoPago:AccessToken")!;
            PublicKey = _configuration.GetValue<string>("MercadoPago:PublicKey")!;
            clientId = _configuration.GetValue<string>("Paypal:ClientId")!;
            secretId = _configuration.GetValue<string>("Paypal:SecretId")!;
        }

        public async Task<string> CrearPago(EnviarPago model)
        {
            model.IdentificationType ??= "CC";
            var myAccessToken = _configuration.GetValue<string>("MercadoPago:AccessToken")!;
            MercadoPagoConfig.AccessToken = myAccessToken;

            //Create request as preference
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>{
                    new PreferenceItemRequest{

                        Id = "1",
                        Title = "Producto 001",
                        Quantity = 2,
                        CurrencyId = "COP",
                        PictureUrl = "https://www.mercadopago.com/org-img/MP3/home/logomp3.gif",
                        UnitPrice = 40000,
                        Description = $"Pago_{model.Ticket.Sorteo.Descripcion}",
                        CategoryId = "2",
                    }
                },
                Payer = new PreferencePayerRequest()
                {
                    Name = "comprador-nuevo@mail.com",
                    Surname = "Doe",
                    Email = "comprador-nuevo@mail.com",
                    Identification = new IdentificationRequest()
                    {
                        Type = model.IdentificationType,
                        Number = "123456789"
                    },
                    Address = new AddressRequest
                    {
                        StreetName = "Calle 123",
                        StreetNumber = model.Ticket.Usuario!.Direccion,
                        ZipCode = "45900",
                    },
                    Phone = new PhoneRequest
                    {
                        Number = "+573196670987",
                        AreaCode = "02"
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://localhost:7048/EstadoPago/Success/",
                    Failure = "https://localhost:7048/EstadoPago/Failure/",
                    Pending = "https://localhost:7048/EstadoPago/Pending/",
                },
                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    ExcludedPaymentMethods = [],
                    ExcludedPaymentTypes = [],
                    Installments = 8
                },
                NotificationUrl = "https://localhost:7048/EstadoPago/Notification/",
                StatementDescriptor = "Sistema de Tickets",
                ExternalReference = $"Referencia_Sorteo_#{Guid.NewGuid()}",
                Expires = true,
                ExpirationDateFrom = DateTime.Now,
                ExpirationDateTo = DateTime.Now.AddMinutes(20)
            };

            //*Create preference using http client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            var convertToJson = JsonConvert.SerializeObject(preference);
            Console.WriteLine(convertToJson);
            return preference.SandboxInitPoint;
            //return preference.InitPoint;

        }

        public async Task<string> CrearPagoPaypal(EnviarPago enviarPago, string? baseUrl)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(BuildRequestBody(Convert.ToDecimal(enviarPago.Ticket.PrecioTicket), baseUrl!, enviarPago.Ticket.Usuario!.Email!));

            var env = new SandboxEnvironment(clientId, secretId);
            //var live = new LiveEnvironment(clientId, secretId);
            var client = new PayPalHttpClient(env);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                var responseBody = response.Result<Order>();

                var approveLink = responseBody.Links.FirstOrDefault(x => x.Rel == "approve");
                if (approveLink != null)
                {
                    return approveLink.Href;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpException e)
            {

                throw;
            }
        }

        private OrderRequest BuildRequestBody(decimal total, string baseUrl, string email)
        {
            total = total * Convert.ToDecimal(0.000237000);
            var request = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD",
                            Value = Convert.ToInt32(total).ToString()
                        }
                    }
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = baseUrl + "/EstadoPago/Success/",
                    CancelUrl = baseUrl + "/EstadoPago/Failure/",
                },
                Payer = new Payer()
                {
                    Email = email,
                }
            };
            return request;
        }

        private async Task<Pago> PagoCreado(int idTicket, decimal total, string emailUsuario)
        {
            var ticket = await _context.Tickets.Include(n => n.Sorteo).FirstOrDefaultAsync(x => x.Id == idTicket);
            if (ticket == null)
            {
                return null;
            }
            ticket.EstadoPago = Helpers.EstadoPago.Pendiente;
            ticket.MontoPagado = total;
            var nuevoPago = new Pago()
            {
                TicketId = idTicket,
                Monto = total,
                MetodoPago = "Paypal",
                EstadoPago = Helpers.EstadoPago.Pendiente,
                DetallesPago = $"Pago - {ticket.NumeroTicket}, {ticket.Sorteo!.Descripcion}"
            };
            await _context.Pagos.AddAsync(nuevoPago);
            await _context.SaveChangesAsync();
            return nuevoPago;
        }
    }
}