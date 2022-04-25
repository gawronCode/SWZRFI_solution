using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;

namespace SWZRFI.Areas.Identity.Pages.Account.ConversationPanel
{
    public partial class CandidatesModel : PageModel
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly ApplicationContext _context;

        public CandidatesModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
             ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }



        public async Task<IActionResult> OnGetAsync()
        {


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }



            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
