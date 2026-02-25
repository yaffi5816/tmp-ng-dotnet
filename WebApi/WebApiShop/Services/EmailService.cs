using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;

namespace Services
{
    public interface IEmailService
    {
        Task SendCodeEmailAsync(string toEmail, string code, string fileName);
    }

    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _appPassword;

        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailSettings:FromEmail"] ?? "yafi3155816@gmail.com";
            _appPassword = configuration["EmailSettings:AppPassword"] ?? "";
        }

        public async Task SendCodeEmailAsync(string toEmail, string code, string fileName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DashGen", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Your Generated Dashboard Code";
            message.Body = new TextPart("plain")
            {
                Text = $"Hello,\n\nHere is your generated dashboard code:\n\n{code}\n\nThank you for using DashGen!"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //await client.AuthenticateAsync("dashgen2026@gmail.com", "kpgitvtdeyigdhok");
                await client.AuthenticateAsync(_fromEmail, _appPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
