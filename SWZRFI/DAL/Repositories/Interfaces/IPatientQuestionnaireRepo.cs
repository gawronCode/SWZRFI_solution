using SWZRFI.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IPatientQuestionnaireRepo : IGeneralRepo<UserQuestionnaire>
    {
        Task<ICollection<UserQuestionnaire>> GetPatientQuestionnairesByEmail(string email);
        Task<ICollection<UserQuestionnaire>> GetPatientQuestionnairesByQuestionnaireId(int id);
        Task<UserQuestionnaire> GetByIdAndUserEmail(int id, string email);
        Task<ICollection<int>> GetUserQuestionnairesIdByEmail(string email);
    }
}
