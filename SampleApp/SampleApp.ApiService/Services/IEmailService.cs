using System.Threading.Tasks;

namespace SampleApp.ApiService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
