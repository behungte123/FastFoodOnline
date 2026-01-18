using System.Threading.Tasks;

namespace FastFood.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}