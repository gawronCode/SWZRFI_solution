using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;
using SWZRFI.DTO.DtoMappingUtils;

namespace SWZRFI.Areas.Identity.Pages.Account.ActionPanel
{
    public class CvPresentationModel : PageModel
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly ApplicationContext _context;


        [BindProperty]
        public CvViewer CvViewer { get; set; }


        public CvPresentationModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo¿na wczytaæ u¿ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var userWithCv = await _context.UserAccount
                .Include(u => u.Cv)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userWithCv.Cv != null)
            {
                CvViewer = userWithCv.Cv.MapToDto();
                CvViewer.Email = userWithCv.Email;
            }
            else
            {
                CvViewer = null;
            }


            return Page();
        }




    }
}
