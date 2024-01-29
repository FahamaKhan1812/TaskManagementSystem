using System.Net.Mail;
using System.Net;

namespace TaskManagementSystem.Application.Services;
internal class EmailService : IEmailService
{
    private readonly string smtpServer = "smtp.gmail.com"; // replace with your SMTP server
    private readonly int smtpPort = 587;
    private readonly string smtpUsername = "fahamakhan3@gmail.com"; // replace with your SMTP username
    private readonly string smtpPassword = "kfnjqmchapwompfr"; // replace with your SMTP password

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient
            {
                Host = smtpServer,
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Handle exceptions accordingly (log, throw, etc.)
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}