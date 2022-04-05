using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;

namespace SWZRFI.Areas.Identity.Pages.Account.ActionPanel
{
    public class ApplicationsModel : PageModel
    {

        private readonly UserManager<UserAccount> _userManager;
        private readonly ApplicationContext _context;

        public ApplicationsModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public IEnumerable<MyApplication> Applications { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo¿na wczytaæ u¿ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var userWithCv = await _context.UserAccount
                .Include(u => u.Cv)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            var applications = await _context
                .Applications.Where(a => a.UserAccountId == user.Id)
                .AsSplitQuery()
                .Include(a => a.Company)
                .Include(a => a.JobOffer)
                .ToListAsync();

            var questionnaireAccess = await _context.QuestionnaireAccesses
                .Where(q => q.UserId == user.Id).ToListAsync();

            if (applications.Any())
                Applications = applications.Select(a => new MyApplication
                {
                    Id = a.Id,
                    JobOfferName = a.JobOffer.Title,
                    JobOfferId = a.JobOffer.Id,
                    CompanyName = a.Company.Name,
                    Status = a.Opened,
                    Date = a.Date,
                });
            else
                Applications = null;

            if (Applications != null && questionnaireAccess != null)
            {
                Applications = Applications.GroupJoin(
                    questionnaireAccess.OrderByDescending(q => q.JobOfferId),
                    a => a.JobOfferId,
                    q => q.JobOfferId,
                    (a, q) =>
                    {
                        a.QuestionnaireAccess = q.FirstOrDefault();
                        return a;
                    }
                    ).ToList();
            }


            return Page();
        }
    }
}
