using Microsoft.AspNetCore.Identity.UI.Services;
using NuGet.Protocol;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace WebApp_Sorteo.Helpers
{
    public class EmailSender(IConfiguration configuration, ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly string SendGridSecret = configuration.GetValue<string>("Sendgrid:SecretKey")!;
        private readonly ILogger<EmailSender> _logger = logger;
        private readonly string email = "deii@mail.com";

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SendGridClient(SendGridSecret);
                var from = new EmailAddress(this.email, "Sistema de Tickets");
                var to = new EmailAddress(email, $"Usuario: {email.ToUpper()}");
                var mensaje = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
                _logger.LogInformation($"Correo enviado: {mensaje.ToJson()}");
                return client.SendEmailAsync(mensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message,ex.ToJson());
                throw;
            }
        }
    }
}
