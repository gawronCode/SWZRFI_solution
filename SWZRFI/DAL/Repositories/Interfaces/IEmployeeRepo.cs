using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IEmployeeRepo
    {
        Task CreateCorporationalInvitation(CorporationalInvitation invitation);
        Task<bool> ValidateAgainstEmailAndGuid(string email, Guid guid);
        Task<CorporationalInvitation> GetCorporationalInvitation(Guid guid);
    }
}
