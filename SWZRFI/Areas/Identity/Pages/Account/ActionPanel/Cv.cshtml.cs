using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;

namespace SWZRFI.Areas.Identity.Pages.Account.ActionPanel
{
    public class CvModel : PageModel
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly ApplicationContext _context;

        public CvModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [BindProperty]
        public Cv Cv { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo�na wczyta� u�ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var userWithCv = await _context.UserAccount
                .Include(u => u.Cv)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userWithCv.Cv != null)
                Cv = userWithCv.Cv;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie mo�na wczyta� u�ytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var userDb = await _context.UserAccount.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (Cv.Id == 0)
            {
                await _context.Cvs.AddAsync(Cv);
                await _context.SaveChangesAsync();
                userDb.CvId = Cv.Id;

                _context.UserAccount.Update(userDb);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            _context.Cvs.Update(Cv);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
