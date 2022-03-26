using SWZRFI.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IUserQuestionnaireAnswerRepo : IGeneralRepo<UserQuestionnaireAnswer>
    {
        Task<ICollection<UserQuestionnaireAnswer>> GetByUserEmailAndQuestionnaireId(string email, int id);
        Task<ICollection<UserQuestionnaireAnswer>> GetByUserEmail(string email);
    }
}
