using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace TestApp.Infrastructure.EmailSender
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string pathToLog)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "login@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder();
            builder.TextBody = "File log of deletions";
            builder.Attachments.Add(pathToLog);
            emailMessage.Body = builder.ToMessageBody();
            
            using ( var client = new SmtpClient() )
            {
                //Проверьте настройку для вашего почтового сервиса
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(  "Ваша почта", "Пароль");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
