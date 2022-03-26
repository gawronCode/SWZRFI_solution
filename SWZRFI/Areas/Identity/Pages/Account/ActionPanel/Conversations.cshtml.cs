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

namespace SWZRFI.Areas.Identity.Pages.Account.ActionPanel
{
    public class ConversationsModel : PageModel
    {

        private readonly UserManager<UserAccount> _userManager;
        private readonly ApplicationContext _context;

        public ConversationsModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [BindProperty]
        public IEnumerable<Conversation> Conversations { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo¿na wczytaæ u¿ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var userAccount = await _context.UserAccount
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            var conversations = await _context.Conversations
                .Where(c => c.FirstInterOcutorId == userAccount.Id || c.SecondInterOcutorId == userAccount.Id)
                .ToListAsync();

            if (conversations == null || !conversations.Any())
                Conversations = null;
            else
                Conversations = conversations;

            return Page();
        }
    }
}
