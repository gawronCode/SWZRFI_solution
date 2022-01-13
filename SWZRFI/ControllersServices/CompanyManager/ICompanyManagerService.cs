using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.ControllersServices.CompanyManager
{
    public interface ICompanyManagerService
    {
        Task CreateCompany(string email, Company company);
        Task<Company> GetUserCompany(string email);
        Task SaveEditedCompany(Company company);
    }
}
