using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SWZRFI.ControllersServices.EmployeeManager;
using SWZRFI.DAL.Models;

namespace SWZRFI.Controllers
{
    public class EmployeeManagerController : BaseController
    {
        private readonly IEmployeeManagerService _employeeManagerService;

        public EmployeeManagerController(IEmployeeManagerService employeeManagerService)
        {
            _employeeManagerService = employeeManagerService;
        }


        public IActionResult Index()
        {
            return View();
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
