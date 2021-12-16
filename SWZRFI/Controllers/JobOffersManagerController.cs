using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWZRFI.DAL.Models;
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


        public JobOffer JobOffer { get; set; }




        public JobOffersManagerController(IJobOffersManagerService jobOffersManagerService)
        {
            _jobOffersManagerService = jobOffersManagerService;
        }

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
        public IActionResult AddSkillsToJobOffer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSkillsToJobOffer(SkillRequirement skillRequirement)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(AddSkillsToJobOffer), skillRequirement);

            JobOffer.SkillRequirements.Add(skillRequirement);

            return RedirectToAction(nameof(AddSkillsToJobOffer));
        }



        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        

    }
}
