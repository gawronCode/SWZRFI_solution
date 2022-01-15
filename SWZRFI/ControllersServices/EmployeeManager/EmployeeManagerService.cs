using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SWZRFI.ConfigData;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI_Utils.EmailHelper;
using SWZRFI_Utils.EmailHelper.Models;

namespace SWZRFI.ControllersServices.EmployeeManager
{
    public class EmployeeManagerService : IEmployeeManagerService
    {

        private readonly IUserRepo _userRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;

        public EmployeeManagerService(IUserRepo userRepo,
                               IEmployeeRepo employeeRepo,
                               IEmailSender emailSender,
                               IConfigGetter configGetter)
        {
            _userRepo = userRepo;
            _emailSender = emailSender;
            _employeeRepo = employeeRepo;
            _configGetter = configGetter;
        }

        public async Task<bool> SendInvitation(string userEmail, string callBackUrl, CorporationalInvitation invitation)
        {
            var user = await _userRepo.GetUserByEmailAsync(userEmail);

            if (user == null || user.CompanyId == null)
                return false;

            invitation.CompanyId = (int)user.CompanyId;

            var emailCredentials = GetEmailCredentials();
            var emailMessage = GetEmailMessage(
                new List<string>() {invitation.Email},
                $"Zaproszenie do współpracy w firmie {user.Company.Name}",
                "Klikając w załączony link, zostaniesz przeniesiony do strony na której będziesz mógł/mogła utworzyć swoje konto pracownika." +
                "Pamiętaj że link jest jednorazowy i ma ograniczony czas aktywności. " +
                "Ze względów bezpieczeństwa możliwe jest założenie konta tylko z adresu email, na jaki została dostarczona ta wiadomość." +
                "Wykorzystanie innego adresu spowoduje dezaktywację linka i uniemożliwi założenie konta do czasu otrzymania kolejnej waidomości." +
                $"<a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>kliknij tutaj aby utworzyć swoje konto</a>");

            await _emailSender.Send(emailCredentials, emailMessage);
            await _employeeRepo.CreateCorporationalInvitation(invitation);
            
            return true;
        }

        private EmailCredentials GetEmailCredentials()
        {
            var identityEmail = _configGetter.GetIdentityEmail();
            return new EmailCredentials
            {
                EmailAddress = identityEmail.Email,
                Password = identityEmail.Password,
                Port = identityEmail.Port,
                SmtpHost = identityEmail.Smtp
            };
        }

        private EmailMessage GetEmailMessage(IEnumerable<string> recipients, string subject, string content)
        {
            return new EmailMessage
            {
                Recipients = recipients,
                Subject = subject,
                Content = content
            };
        }



    }
}
