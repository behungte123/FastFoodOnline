using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FastFood.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            string fromMail = emailSettings["Mail"];
            string password = emailSettings["Password"];
            string host = emailSettings["Host"];
            int port = int.Parse(emailSettings["Port"]);

            var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fromMail, password)
            };

            var mailMessage = new MailMessage(fromMail, toEmail, subject, message)
            {
                IsBodyHtml = true // Cho phép gửi nội dung HTML
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}