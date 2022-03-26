using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.ControllersServices.CompanyManager;
using SWZRFI.DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class CompanyManagerController : BaseController
    {

        private readonly ICompanyManagerService _companyManagerService;

        public CompanyManagerController(ICompanyManagerService companyManagerService)
        {
            _companyManagerService = companyManagerService;
        }

        public async Task<IActionResult> Index()
        {
            var company = await _companyManagerService.GetUserCompany(GetCurrentUserEmail());

            return View(company);
        }

        [HttpGet]
        public async Task<IActionResult> EditCompany()
        {
            var company = await _companyManagerService.GetUserCompany(GetCurrentUserEmail());

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompany(Company incomingCompany)
        {
            var company = await _companyManagerService.GetUserCompany(GetCurrentUserEmail());
            await TryUpdateModelAsync(company);

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditCompany), company);

            await _companyManagerService.SaveEditedCompany(company);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CreateCompany), company);

            await _companyManagerService.CreateCompany(GetCurrentUserEmail(), company);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteCompany()
        {
            return View();
        }
        
    }
}
