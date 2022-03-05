using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;
using System;
using System.Threading.Tasks;

namespace SWZRFI.Controllers
{
    public class ApplicationController : BaseController
    {

        private readonly ApplicationContext _context;

        public ApplicationController(ApplicationContext context)
        {
            _context = context;
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
                JobOfferId = jobOffer.Id
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return View("Confirm", $"Pomyślnie aplikowano na ofertę {jobOffer.Title}!");
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
