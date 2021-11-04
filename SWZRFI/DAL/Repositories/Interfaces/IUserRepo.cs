using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task DeleteUnverifiedAccountsAsync();
    }
}