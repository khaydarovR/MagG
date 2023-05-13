using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit.Text;

namespace Mag.BL;

public class EmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var emailMessage = new MimeMessage();
 
        emailMessage.From.Add(new MailboxAddress("Администрация сайта", "razil-khayka@yandex.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
             
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.yandex.ru", 465, true);
            await client.AuthenticateAsync("razil-khayka@yandex.ru", "svybrclzkaqslguq");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}