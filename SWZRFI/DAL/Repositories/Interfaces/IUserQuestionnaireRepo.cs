using SWZRFI.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IUserQuestionnaireRepo : IGeneralRepo<UserQuestionnaire>
    {
        Task<ICollection<UserQuestionnaire>> GetUserQuestionnairesByEmail(string email);
        Task<ICollection<UserQuestionnaire>> GetUserQuestionnairesByQuestionnaireId(int id);
        Task<UserQuestionnaire> GetByIdAndUserEmail(int id, string email);
        Task<ICollection<int>> GetUserQuestionnairesIdByEmail(string email);
    }
}
