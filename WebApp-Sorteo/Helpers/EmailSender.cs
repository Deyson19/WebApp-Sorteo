using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace WebApp_Sorteo.Helpers
{
    public class EmailSender(IConfiguration configuration) : IEmailSender
    {
        private readonly string SendGridSecret = configuration.GetValue<string>("Sendgrid:SecretKey")!;
        private readonly string email = "deii@mail.com";

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress(this.email, "Sistema de Tickets");
            var to = new EmailAddress(email, $"Usuario: {email.ToUpper()}");
            var mensaje = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(mensaje);
        }
    }
}
