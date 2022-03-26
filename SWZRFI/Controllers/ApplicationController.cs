using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.ConfigData;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;
using SWZRFI_Utils.EmailHelper;
using SWZRFI_Utils.EmailHelper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.Controllers
{
    public class ApplicationController : BaseController
    {

        private readonly ApplicationContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;

        public ApplicationController(ApplicationContext context,
            IEmailSender emailSender,
            IConfigGetter configGetter)
        {
            _context = context;
            _emailSender = emailSender;
            _configGetter = configGetter;
        }


        public async Task<IActionResult> Confirm(int id)
        {
            var email = GetCurrentUserEmail();
            var jobOffer = await _context.JobOffers.Include(j => j.Company).FirstOrDefaultAsync(j => j.Id == id);
            var user = await _context.UserAccount.Include(u => u.Cv).FirstOrDefaultAsync(u => u.Email == email);
            


            if (user.Cv == null)
                return View("Confirm", "Brak CV, przejdź do zakładki \"moje konto\" aby dodać CV");

            if (await _context.Applications.AnyAsync(a => a.CvId == user.CvId && a.JobOfferId == jobOffer.Id))
                return View("Confirm", "Nie można aplikować 2 razy na tą samą ofertę!");


            var application = new Application
            {
                Date = DateTime.Now,
                CvId = (int)user.CvId,
                CompanyId = jobOffer.CompanyId,
                Opened = false,
                JobOfferId = jobOffer.Id,
                UserAccountId = user.Id                
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            await SendEmail(user.Email, jobOffer.Title);

            return View("Confirm", $"Pomyślnie aplikowano na ofertę {jobOffer.Title}!");
        }

        private async Task SendEmail(string email, string jobOffer)
        {
            var identityEmail = _configGetter.GetIdentityEmail();
            var emailCredentials = new EmailCredentials
            {
                EmailAddress = identityEmail.Email,
                Password = identityEmail.Password,
                Port = identityEmail.Port,
                SmtpHost = identityEmail.Smtp
            };

            var emailMessage = new EmailMessage
            {
                Recipients = new List<string>() { email },
                Subject = $"Potwierdzenie złożenia aplikacji SWZRFI",
                Content =
                    $"Własnie aplikowałeś na ofertę {jobOffer}! Status aplikacji możesz śledzić na swoim koncie, otrzymasz również powiadomienie email gdy status aplikacji ulegnie zmianie."
            };


            await _emailSender.Send(emailCredentials, emailMessage);
        }

        public async Task<IActionResult> ApplicationConfirmation(int id)
        {
            var jobOffer = await _context.JobOffers.Include(j => j.Company).FirstOrDefaultAsync(j => j.Id == id);

            var model = new ApplicationConfirmation
            {
                JobOfferId = jobOffer.Id,
                JobOfferName = jobOffer.Title
            };

            return View(model);
        }
    }
}
