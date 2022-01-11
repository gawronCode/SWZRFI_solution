﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.ControllersServices.CompanyManager;
using SWZRFI.DAL.Models;

namespace SWZRFI.Controllers
{
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

        public IActionResult EditCompany()
        {
            return View();
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

        public IActionResult ManageEmployees()
        {
            return View();
        }

        public IActionResult SendInvitationCodeToUser()
        {
            return RedirectToAction(nameof(ManageEmployees));
        }

    }
}
