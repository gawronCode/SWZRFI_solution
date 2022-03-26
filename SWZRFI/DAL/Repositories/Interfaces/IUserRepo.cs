using System.Collections.Generic;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;
using SWZRFI.DTO;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task DeleteUnverifiedAccountsAsync();
        Task<UserAccount> GetUserByEmailAsync(string email);
        Task UpdateUser(UserAccount userAccount);
        Task<IEnumerable<UserRoles>> GetUsersRolesForCompany(int companyId);
        Task<IEnumerable<UserAccount>> GetAllForCompany(int companyId);
    }
}