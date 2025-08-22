using CustomerManagementApp.Models;
using System.Net;
using System.Net.Mail;

namespace CustomerManagementApp.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public SmtpEmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, IFormFile attachment = null)
        {
            try
            {
                var getEmailSetting = _config.GetSection("EmailSettings");

                // Create the email
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(getEmailSetting["From"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                // ✅ Add attachment if provided
                if (attachment != null && attachment.Length > 0)
                {
                    var ms = new MemoryStream();
                    await attachment.CopyToAsync(ms);
                    ms.Position = 0;

                    // Ensure safe filename (avoid invalid chars)
                    var safeFileName = Path.GetFileName(attachment.FileName);

                    // Add with content type if available
                    mailMessage.Attachments.Add(
                        new Attachment(ms, safeFileName, attachment.ContentType.ToString())
                    );
                }

                // Configure SMTP client
                using var smtpClient = new SmtpClient(getEmailSetting["SmtpServer"])
                {
                    Port = int.Parse(getEmailSetting["Port"] ?? "587"),
                    Credentials = new NetworkCredential(
                        getEmailSetting["Username"],   // Gmail address
                        getEmailSetting["Password"]    // Gmail App Password
                    ),
                    EnableSsl = bool.Parse(getEmailSetting["EnableSSL"] ?? "true")
                };

                // Send email
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log error here (e.g., to DB, file, or console)
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

    }
}
