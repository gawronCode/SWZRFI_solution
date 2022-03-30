using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DTO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DTO.DtoMappingUtils;
using SWZRFI.DAL.Models;

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
            cvViewModel.JobOfferId = application.JobOfferId;
            cvViewModel.UserId = application.UserAccountId;

            application.Opened = true;
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();


            return View(cvViewModel);
        }

        public async Task<IActionResult> SendQuestionnaireInvitation(string id)
        {
            var data = id.Split(',');
            var user = await _context.UserAccount.FirstOrDefaultAsync(q => q.Id == data[0]);
            var jobOffer = await _context.JobOffers.Include(q => q.Questionnaire).FirstOrDefaultAsync(q => q.Id == int.Parse(data[1]));

            if(jobOffer.QuestionnaireId == null)
                return RedirectToAction(nameof(Index));


            var invitation = new QuestionnaireAccess
            {
                Expired = false,
                QuestionnaireId = (int)jobOffer.QuestionnaireId,
                JobOfferId = jobOffer.Id,
                QuestionnaireTitle = jobOffer.Questionnaire.Name,
                UserId = user.Id
            };

            _context.QuestionnaireAccesses.Add(invitation);
            _context.UserQuestionnaires.Add(new UserQuestionnaire 
            { 
                PatientEmail = user.Email, 
                QuestionnaireId = jobOffer.Questionnaire.Id 
            });


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

    }
}
