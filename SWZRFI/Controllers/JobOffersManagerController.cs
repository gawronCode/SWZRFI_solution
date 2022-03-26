using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SWZRFI.ControllersServices.JobOffersManager;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;
using SWZRFI.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class JobOffersManagerController : BaseController
    {

        private readonly IJobOffersManagerService _jobOffersManagerService;
        private readonly ApplicationContext _context;

        public JobOffersManagerController(
            IJobOffersManagerService jobOffersManagerService,
            ApplicationContext context)
        {
            _jobOffersManagerService = jobOffersManagerService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _jobOffersManagerService.GetIndexPageData(GetCurrentUserEmail()));
        }

        [HttpGet]
        public IActionResult CreateJobOffer()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Applications(int id)
        {
            var application = await _context.Applications.Include(a => a.Cv).FirstOrDefaultAsync(a => a.JobOfferId == id);
            var userThatApplied = await _context.UserAccount.FirstOrDefaultAsync(u => u.CvId == application.CvId);

            application.Opened = true;
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();



            return View();
        }


        public async Task<IActionResult> AssignQuizToJobOffer(JobOfferQuizSelector value)
        {

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> SelectQuiz(int id)
        {
            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(q => q.Id == id);
            var quizzes = await _context.Questionnaires.Where(q => q.CompanyId != null && q.CompanyId == jobOffer.CompanyId).ToListAsync();

            var model = new JobOfferQuizSelector
            {
                JobOfferId = id,
                Questionnaires = quizzes.Select(q => new QuizSelected
                {
                    QuizId = q.Id,
                    Name = q.Name,
                    Selected = false
                }).ToList()
            };
            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> CreateJobOffer(JobOffer jobOffer)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CreateJobOffer), jobOffer);

            await _jobOffersManagerService.CreateJobOffer(GetCurrentUserEmail(), jobOffer);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditJobOffer(int id)
        {
            var jobOffer = await _jobOffersManagerService.GetJobOfferForEdition(id);
            return View(jobOffer);
        }

        [HttpPost]
        public async Task<IActionResult> EditJobOffer(JobOffer incomingJobOffer)
        {

            var jobOffer = await _jobOffersManagerService.GetJobOfferForEdition(incomingJobOffer.Id);
            await TryUpdateModelAsync(jobOffer);


            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditJobOffer), jobOffer);

            await _jobOffersManagerService.SaveEditedJobOffer(GetCurrentUserEmail(),jobOffer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveJobOffer(int id)
        {
            var jobOffer = await _jobOffersManagerService.GetJobOfferForEdition(id);
            var confirmRemoveByName = new ConfirmRemoveByName
            {
                ObjectName = jobOffer.Title
            };

            return RedirectToAction(nameof(ConfirmRemove), confirmRemoveByName);
        }

        public async Task<IActionResult> ConfirmRemove(ConfirmRemoveByName confirmRemoveByName)
        {
            if (!ModelState.IsValid)
                return View(confirmRemoveByName);

            if (await _jobOffersManagerService.RemoveJobOffer(GetCurrentUserEmail(), confirmRemoveByName.ObjectId))
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(ErrorMessage));
        }

        public IActionResult ErrorMessage()
        {
            return View();
        }

    }
}
