using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SWZRFI.ConfigData;
using SWZRFI.DAL.Models.IdentityModels;
using SWZRFI_Utils.EmailHelper;
using SWZRFI_Utils.EmailHelper.Models;

namespace SWZRFI.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;

        public EmailModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            IEmailSender emailSender,
            IConfigGetter configGetter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _configGetter = configGetter;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Proszę podać nowy adres email")]
            [EmailAddress]
            [Display(Name = "Nowy adres email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(UserAccount user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można wczytać użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można wczytać użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);

                var identityEmail = _configGetter.GetIdentityEmail();
                var emailCredentials = new EmailCredentials
                {
                    EmailAddress = identityEmail.Email,
                    Password = identityEmail.Password,
                    Port = identityEmail.Port,
                    SmtpHost = identityEmail.Smtp
                };

                var emailMessage = new EmailMessage
                {
                    Recipients = new List<string>() { Input.NewEmail },
                    Subject = "Potwierdzenie adresu email SWZRFI",
                    Content =
                        $"Aby potwierdzić podany adres email kliknij załączony link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj</a>."
                };


                await _emailSender.Send(emailCredentials, emailMessage);


                StatusMessage = "Na podany adres email został przesłany linka aktywacyjny.";
                return RedirectToPage();
            }

            StatusMessage = "Nie udało się zmienić adresu email.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);

            var identityEmail = _configGetter.GetIdentityEmail();
            var emailCredentials = new EmailCredentials
            {
                EmailAddress = identityEmail.Email,
                Password = identityEmail.Password,
                Port = identityEmail.Port,
                SmtpHost = identityEmail.Smtp
            };

            var emailMessage = new EmailMessage
            {
                Recipients = new List<string>() { email },
                Subject = "Potwierdzenie adresu email SWZRFI",
                Content =
                    $"Aby potwierdzić podany adres email kliknij załączony link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj</a>."
            };


            await _emailSender.Send(emailCredentials, emailMessage);

            StatusMessage = "Na podany adres email został przesłany linka aktywacyjny.";
            return RedirectToPage();
        }
    }
}
