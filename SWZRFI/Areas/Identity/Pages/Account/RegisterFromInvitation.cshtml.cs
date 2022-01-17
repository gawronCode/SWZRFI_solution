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
using SWZRFI.ControllersServices.EmployeeManager;
using SWZRFI.DAL.Enums;
using SWZRFI.DAL.Models;
using SWZRFI_Utils.EmailHelper;
using SWZRFI_Utils.EmailHelper.Models;

namespace SWZRFI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterFromInvitationModel : PageModel
    {
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly ILogger<RegisterFromInvitationModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;
        private readonly IEmployeeManagerService _employeeManagerService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterFromInvitationModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ILogger<RegisterFromInvitationModel> logger,
            IEmailSender emailSender,
            IConfigGetter configGetter,
            IEmployeeManagerService employeeManagerService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _configGetter = configGetter;
            _employeeManagerService = employeeManagerService;
            _roleManager = roleManager;
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
            [Required(ErrorMessage = "Proszę podać kod zaproszenia")]
            [DataType(DataType.Text)]
            [Display(Name = "Kod zaproszenia")]
            public string InvitationCode { get; set; }
        }

        public async Task OnGetAsync(string code, string returnUrl = null)
        {
            Input = new InputModel
            {
                InvitationCode = Encoding.Default.GetString(WebEncoders.Base64UrlDecode(code))
            };
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string code, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid) return Page();

            Guid guid;

            try
            {
                guid = Guid.Parse(Input.InvitationCode);
            }
            catch
            {
                guid = Guid.Empty;
            }

            var validation = await _employeeManagerService.ValidatePreRegistration(Input.Email, guid);

            if (!validation)             
                return Page();

            var corporationalInvitation = await _employeeManagerService.GetCorporationalInvitation(guid);

            var user = new UserAccount
            {
                UserName = Input.Email,
                Email = Input.Email,
                RegistrationDate = DateTime.Now,
                CompanyId = corporationalInvitation.CompanyId
            };

            var createResutl = await _userManager.CreateAsync(user, Input.Password);

            if (!createResutl.Succeeded) 
                return Page();

            await _userManager.AddToRoleAsync(user, GetRoles(-1));
            await _userManager.AddToRoleAsync(user, GetRoles(corporationalInvitation.AssignedRole));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new {area = "Identity", userId = user.Id, code = token, returnUrl = returnUrl},
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
                return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl = returnUrl});
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        private string GetRoles(int role)
        {
            return role switch
            {
                0 => Roles.RecruitersAccount.ToString("G"),
                1 => Roles.ManagerAccount.ToString("G"),
                _ => Roles.PersonalAccount.ToString("G")
            };
        }


    }
}
