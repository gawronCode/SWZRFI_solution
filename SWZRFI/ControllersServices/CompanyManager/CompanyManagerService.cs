using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI_Utils.EmailHelper;
using SWZRFI_Utils.EmailHelper.Models;

namespace SWZRFI.ControllersServices.CompanyManager
{
    public class CompanyManagerService : ICompanyManagerService
    {

        private readonly IUserRepo _userRepo;
        private readonly ICompanyRepo _companyRepo;
        private readonly IEmailSender _emailSender;

        public CompanyManagerService(IUserRepo userRepo,
                                     ICompanyRepo companyRepo,
                                     IEmailSender emailSender)
        {
            _userRepo = userRepo;
            _companyRepo = companyRepo;
            _emailSender = emailSender;
        }

        public async Task<Company> GetUserCompany(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            
            return user.Company;
        }

        public async Task CreateCompany(string email, Company company)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            
            if(user.Company != null)
                return;

            user.CompanyId = await _companyRepo.CreateCompany(company);
            await _userRepo.UpdateUser(user);
        }

        public async Task SaveEditedCompany(Company company)
        {
            await _companyRepo.UpdateCompany(company);
        }
    }
}
