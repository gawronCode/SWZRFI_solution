using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SWZRFI.DTO.ViewModels;
using SWZRFI.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DAL.Models;
using System.Linq;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class UserPermissionsController : BaseController
    {

        private readonly IUserRepo _userRepo;
        private readonly UserManager<UserAccount> _userManager;

        public UserPermissionsController(IUserRepo userRepo,
            UserManager<UserAccount> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }


        public async Task<ActionResult> Index()
        {

            var email = GetCurrentUserEmail();
            var currentUser = await _userRepo.GetUserByEmailAsync(email);
            var companyUsers = await _userRepo.GetAllForCompany((int)currentUser.CompanyId);
            var model = companyUsers.Select(async u => new EmployeeStatusViewModel
            {
                EmailAddress = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsManager = await _userManager.IsInRoleAsync(u, "RecruitersAccount")
            }).ToList();

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
    }
}
