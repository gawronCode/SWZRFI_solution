using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task DeleteUnverifiedAccountsAsync();
        Task<UserAccount> GetUserByEmailAsync(string email);
    }
}