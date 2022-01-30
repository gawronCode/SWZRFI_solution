using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;

namespace SWZRFI.ControllersServices.EmployeeManager
{
    public interface IEmployeeManagerService
    {
        Task<bool> SendInvitation(string userEmail, string callBackUrl, CorporationalInvitation invitation);
        Task<bool> ValidatePreRegistration(string email, Guid guid);
        Task<CorporationalInvitation> GetCorporationalInvitation(Guid guid);
        Task<IEnumerable<UserRoles>> GetUserRoles(string userEmail);
    }
}
