using SWZRFI.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IUserAnswerRepo : IGeneralRepo<UserAnswer>
    {
        public Task<ICollection<UserAnswer>> GetUserAnswersByUserEmail(string email);
    }
}
