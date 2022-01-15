using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SWZRFI_Utils.EmailHelper.Models;


namespace SWZRFI_Utils.EmailHelper
{
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Metoda pozwala na wysyłanie wiadomości email do wielu adresatów za pomocą konta email
        /// </summary>
        /// <param name="emailCredentials">Dane konta pocztowego z którego ma zostać wysłana wiadomość</param>
        /// <param name="emailMessage">Wiadomość oraz lista adresatów</param>
        /// <returns>Task do egzekucji asynchronicznej</returns>
        public async Task Send(EmailCredentials emailCredentials, EmailMessage emailMessage)
        {
            await Task.Run(() =>
            {
                var email = CreateEmail(emailCredentials, emailMessage);

                using var smtpClient = new SmtpClient();
                
                smtpClient.Connect(emailCredentials.SmtpHost, emailCredentials.Port, SecureSocketOptions.Auto);
                smtpClient.Authenticate(emailCredentials.EmailAddress, emailCredentials.Password);
                smtpClient.Send(email);
                smtpClient.Disconnect(true);
            });
        }

        
        private MimeMessage CreateEmail(EmailCredentials emailCredentials, EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailCredentials.EmailAddress));
            foreach (var recipient in emailMessage.Recipients)
                email.To.Add(MailboxAddress.Parse(recipient));
            
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailMessage.Content };

            return email;
        }

    }
}
