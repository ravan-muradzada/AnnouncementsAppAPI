using Application.ExternalServiceInterfaces;
using Domain.CustomExceptions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly string? _fromEmail;
        private readonly string? _appPassword;
        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailServiceConfiguration:FromEmail"];
            _appPassword = configuration["EmailServiceConfiguration:AppPassword"];
        }
        public async Task SendEmail(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_fromEmail));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_fromEmail, _appPassword);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new FailedToSendEmailException(ex.Message);
            }
        }
    }
}
