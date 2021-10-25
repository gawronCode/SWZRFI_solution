using System.Threading.Tasks;
using SWZRFI_Utils.EmailHelper.Models;

namespace SWZRFI_Utils.EmailHelper
{
    public interface IEmailSender
    {
        /// <summary>
        /// Metoda pozwala na wysyłanie wiadomości email do wielu adresatów za pomocą konta email
        /// </summary>
        /// <param name="emailCredentials">Dane konta pocztowego z którego ma zostać wysłana wiadomość</param>
        /// <param name="emailMessage">Wiadomość oraz lista adresatów</param>
        /// <returns>Task do egzekucji asynchronicznej</returns>
        Task Send(EmailCredentials emailCredentials, EmailMessage emailMessage);
    }
}