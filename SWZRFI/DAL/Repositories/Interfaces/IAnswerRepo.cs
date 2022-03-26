using SWZRFI.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IAnswerRepo : IGeneralRepo<Answer>
    {
        public Task<ICollection<Answer>> GetAnswersByQuestionId(int id);
    }
}
