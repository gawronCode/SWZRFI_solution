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

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class EmployeeManagerController : BaseController
    {
        private readonly IEmployeeManagerService _employeeManagerService;

        public EmployeeManagerController(IEmployeeManagerService employeeManagerService)
        {
            _employeeManagerService = employeeManagerService;
        }


        public async Task<IActionResult> Index()
        {
            var userRoles = await _employeeManagerService.GetUserRoles(GetCurrentUserEmail());
            return View(userRoles);
        }



        public IActionResult EditUserRoles(UserRoles userRoles)
        {
            return View(userRoles);
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
