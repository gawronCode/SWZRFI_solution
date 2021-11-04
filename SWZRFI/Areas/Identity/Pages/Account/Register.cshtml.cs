using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SWZRFI.ConfigData;
using SWZRFI.DAL.Models.IdentityModels;
using SWZRFI_Utils.EmailHelper;
using SWZRFI_Utils.EmailHelper.Models;

namespace SWZRFI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;

        public RegisterModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfigGetter configGetter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _configGetter = configGetter;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "Podanie imienia jest wymagane")]
            [DataType(DataType.Text)]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Podanie nazwiska jest wymagane")]
            [DataType(DataType.Text)]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Proszę podać adres email")]
            [EmailAddress]
            [Display(Name = "Adres email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Proszę podać hasło")]
            [StringLength(50, ErrorMessage = "Hasło musi zawierać przynajmniej 6 znaków i być niedłuższe niż 50 znaków", MinimumLength = 6)]
            [DataType(DataType.Password, ErrorMessage = "Hasło musi zawierać przynajmniej jedną wielką literę, znak specjalny, oraz cyfrę")]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdzenie hasła")]
            [Compare("Password", ErrorMessage = "Błędne hasło")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid) return Page();
            var user = new UserAccount { UserName = Input.Email, Email = Input.Email, RegistrationDate = DateTime.Now};
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                    

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
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
                    Recipients = new List<string>() {Input.Email},
                    Subject = "Potwierdzenie adresu email SWZRFI",
                    Content =
                        $"Aby potwierdzić podany adres email kliknij załączony link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj</a>."
                };


                await _emailSender.Send(emailCredentials, emailMessage);

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            
            return Page();
        }
    }
}
