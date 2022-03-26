using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SWZRFI.ControllersServices.EmployeeManager;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DTO.ViewModels;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class EmployeeManagerController : BaseController
    {
        private readonly IEmployeeManagerService _employeeManagerService;
        private readonly IUserRepo _userRepo;
        private readonly UserManager<UserAccount> _userManager;

        public EmployeeManagerController(IEmployeeManagerService employeeManagerService, 
            IUserRepo userRepo,
            UserManager<UserAccount> userManager)
        {
            _employeeManagerService = employeeManagerService;
            _userRepo = userRepo;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var userRoles = await _employeeManagerService.GetUserRoles(GetCurrentUserEmail());
            return View(userRoles);
        }



        public async Task<IActionResult> EditUserRoles()
        {
            var email = GetCurrentUserEmail();
            var currentUser = await _userRepo.GetUserByEmailAsync(email);
            var companyUsers = await _userRepo.GetAllForCompany((int)currentUser.CompanyId);
            var model = (await Task.WhenAll(companyUsers.Select(async u => new EmployeeStatusViewModel
            {
                EmailAddress = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsManager = await _userManager.IsInRoleAsync(u, "RecruitersAccount")
            }).ToList())).ToList();

            return View(model);
        }


        public async Task<ActionResult> AddPermissions(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            await _userManager.AddToRoleAsync(user, "RecruitersAccount");

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RemovePermissions(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "RecruitersAccount");

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult SendInvitationCodeToUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendInvitationCodeToUser(CorporationalInvitation corporationalInvitation)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(SendInvitationCodeToUser), corporationalInvitation);

            var callbackUrl = GetCallBackString(corporationalInvitation);
            var result = await _employeeManagerService.SendInvitation(GetCurrentUserEmail(), callbackUrl, corporationalInvitation);

            if (result)
                return RedirectToAction(nameof(SuccessMessageEmployee));

            return RedirectToAction(nameof(SuccessMessageEmployee));
        }

        [HttpGet]
        public IActionResult SuccessMessageEmployee()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ErrorMessageEmployee()
        {
            return View();
        }

        private string GetCallBackString(CorporationalInvitation corporationalInvitation)
        {
            corporationalInvitation.Guid = Guid.NewGuid();
            var returnUrl = Url.Content("~/");
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(corporationalInvitation.Guid.ToString()));
            var callbackUrl = Url.Page(
                "/Account/RegisterFromInvitation",
                pageHandler: null,
                values: new
                {
                    area = "Identity", 
                    code = code, 
                    returnUrl = returnUrl
                },
                protocol: Request.Scheme);
            return callbackUrl;
        }

    }
}
