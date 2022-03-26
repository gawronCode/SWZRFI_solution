using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.Controllers
{
    public class CandidatesController : BaseController
    {
        private readonly ApplicationContext _context;

        public CandidatesController(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var userEmail = GetCurrentUserEmail();
            var company = await _context.Companies.AsSplitQuery()
                .Include(c => c.JobOffers)
                .ThenInclude(j => j.Applications)
                .ThenInclude(a => a.UserAccount).ToListAsync();


            var jobOfferApplications = company
                .SelectMany(c => c.JobOffers)
                .Select(j => new JobOfferApplication
                {
                    JobOffer = j,
                    CandidateApplications = j.Applications.Select(a => new CandidateApplication
                    {
                        Application = a,
                        Cv = a.Cv,
                        UserAccount = a.UserAccount
                    }).ToList()
                }).ToList();


            return View(jobOfferApplications);
        }
    }
}
