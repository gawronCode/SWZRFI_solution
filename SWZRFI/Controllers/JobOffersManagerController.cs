using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DTO.BindingModels;
using SWZRFI.ViewServices.JobOffers;
using SWZRFI.ViewServices.JobOffersManager;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "PersonalAccount, SystemAdmin")]
    public class JobOffersManagerController : Controller
    {

        private readonly IJobOffersManagerService _jobOffersManagerService;

        [BindProperty]
        public JobOfferB Input { get; set; }


        public JobOffersManagerController(IJobOffersManagerService jobOffersManagerService)
        {
            _jobOffersManagerService = jobOffersManagerService;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _jobOffersManagerService.GetIndexPageData(GetCurrentUserEmail()));
        }

        public async Task<IActionResult> CreateJobOffer()
        {

            return View();
        }


        public async Task<IActionResult> Create(JobOfferB jobOffer)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CreateJobOffer), jobOffer);

            return RedirectToAction(nameof(Index));
        }

        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        

    }
}
