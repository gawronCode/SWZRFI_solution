using SWZRFI.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IQuestionRepo : IGeneralRepo<Question>
    {
        Task<ICollection<Question>> GetQuestionsByQuestionnaireId(int id);
    }
}
