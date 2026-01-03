using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration; 
using System.Net;
using System.Net.Mail; 
using System.Threading.Tasks;

namespace ECommerceProjesi.WebUI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string host = _configuration["EmailSettings:Host"];
            int port = int.Parse(_configuration["EmailSettings:Port"]);
            string gonderenEmail = _configuration["EmailSettings:Email"];
            string sifre = _configuration["EmailSettings:Password"];
            var client = new SmtpClient(host, port)
            {
                EnableSsl = true, 
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(gonderenEmail, sifre)
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(gonderenEmail, "Core Shop"), 
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true 
            };
            mailMessage.To.Add(email);
            await client.SendMailAsync(mailMessage);
        }
    }
}