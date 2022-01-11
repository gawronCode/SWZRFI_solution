using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface ICompanyRepo
    {
        Task<int> CreateCompany(Company company);
    }
}
