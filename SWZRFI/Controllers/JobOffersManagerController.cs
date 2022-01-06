using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DAL.Models;
using SWZRFI.ViewServices.JobOffersManager;


namespace SWZRFI.Controllers
{
    [Authorize(Roles = "PersonalAccount, SystemAdmin")]
    public class JobOffersManagerController : BaseController
    {

        private readonly IJobOffersManagerService _jobOffersManagerService;

        public JobOffersManagerController(IJobOffersManagerService jobOffersManagerService)
        {
            _jobOffersManagerService = jobOffersManagerService;
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
        public async Task<IActionResult> EditJobOffer(JobOffer jobOffer)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditJobOffer), jobOffer);

            await _jobOffersManagerService.SaveEditedJobOffer(GetCurrentUserEmail(),jobOffer);

            return RedirectToAction(nameof(Index));
        }

    }
}
