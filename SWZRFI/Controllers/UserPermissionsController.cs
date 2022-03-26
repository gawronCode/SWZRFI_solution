using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SWZRFI.DTO.ViewModels;
using SWZRFI.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DAL.Models;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class UserPermissionsController : Controller
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
            var patients = await _userRepo.GetAll();
            var model = new List<DoctorViewModel>();

            foreach (var patient in patients)
            {
                
                    model.Add(new DoctorViewModel
                    {
                        EmailAddress = patient.Email,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        IsConfirmed = (await _userManager.IsInRoleAsync(patient, "Doctor"))
                    });
            }

            return View(model);
        }

        public async Task<ActionResult> AddPermissions(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Patient");
            await _userManager.AddToRoleAsync(user, "Doctor");

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RemovePermissions(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Doctor");
            await _userManager.AddToRoleAsync(user, "Patient");

            return RedirectToAction(nameof(Index));
        }
    }
}
