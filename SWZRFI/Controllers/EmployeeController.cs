using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.Controllers
{
    public class EmployeeController : Controller
    {
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
        public IActionResult SendInvitationCodeToUser(CorporationalInvitation corporationalInvitation)
        {
            var returnUrl = @"https://localhost:5001/Identity";
            var callbackUrl = Url.Page(
                "/Account/RegisterFromInvitation",
                pageHandler: null,
                values: new { area = "Identity", code = corporationalInvitation.Guid.ToString(), returnUrl = returnUrl },
                protocol: Request.Scheme);

            return View();
        }

        [HttpGet]
        public IActionResult SuccessMessageEmployee(string message)
        {
            return View();
        }

    }
}
