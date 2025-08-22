namespace CustomerManagementApp.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body, IFormFile attachment = null);
    }
}
