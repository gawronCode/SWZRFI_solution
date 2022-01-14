using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using SWZRFI.ConfigData;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI_Utils.EmailHelper;

namespace SWZRFI.ControllersServices.Employee
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IUserRepo _userRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;

        public EmployeeService(IUserRepo userRepo,
                               IEmployeeRepo employeeRepo,
                               IEmailSender emailSender,
                               IConfigGetter configGetter)
        {
            _userRepo = userRepo;
            _emailSender = emailSender;
            _employeeRepo = employeeRepo;
        }

        public async Task<bool> SendInvitation(string userEmail, CorporationalInvitation invitation)
        {
            var user = await _userRepo.GetUserByEmailAsync(userEmail);


            throw new NotImplementedException();
        }

        private string GetCallbackUrl()
        {
            var returnUrl = @"https://localhost:5001/Identity/Account/RegisterFromInvitation";

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);
        }



    }
}
