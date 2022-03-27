using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DTO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DTO.DtoMappingUtils;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
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
            var currentUser = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == userEmail);

            var company = await _context.Companies.Where(c => c.Id == currentUser.CompanyId).AsSplitQuery()
                .Include(c => c.JobOffers)
                .ThenInclude(j => j.Applications)
                .ThenInclude(a => a.UserAccount)
                .ThenInclude(u => u.Cv).ToListAsync();


            var jobOfferApplications = company
                .SelectMany(c => c.JobOffers)
                .Select(j => new JobOfferApplication
                {
                    JobOffer = j,
                    CandidateApplications = j.Applications.Select(a => new CandidateApplication
                    {
                        Application = a,
                        Cv = a.UserAccount.Cv,
                        UserAccount = a.UserAccount
                    }).ToList()
                }).ToList();

            var model = jobOfferApplications.GroupBy(j => j.JobOffer.Id).ToList();


            return View(jobOfferApplications);
        }

        public async Task<IActionResult> CandidateApplication(int id)
        {
            var application = await _context.Applications
                .Include(q => q.Cv)
                .Include(q => q.UserAccount)
                .FirstOrDefaultAsync(q => q.Id == id);

            var cvViewModel = application.Cv.MapToDto();
            cvViewModel.Email = application.UserAccount.Email;

            application.Opened = true;
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();


            return View(cvViewModel);
        }


    }
}
