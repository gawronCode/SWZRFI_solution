using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DTO.TmpModels;
using SWZRFI.DTO.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class QuestionnaireResultsController : BaseController
    {
        private readonly IQuestionnaireRepo _questionnaireRepo;
        private readonly IQuestionRepo _questionRepo;
        private readonly IUserRepo _userRepo;
        private readonly IUserQuestionnaireRepo _patientQuestionnaireRepo;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IUserAnswerRepo _userAnswerRepo;
        private readonly IUserQuestionnaireAnswerRepo _userQuestionnaireAnswerRepo;
        private readonly ApplicationContext _context;

        public QuestionnaireResultsController(IQuestionnaireRepo questionnaireRepo,
            IQuestionRepo questionRepo,
            IUserRepo userRepo,
            IUserQuestionnaireRepo patientQuestionnaireRepo,
            UserManager<UserAccount> userManager,
            IUserAnswerRepo userAnswerRepo,
            IUserQuestionnaireAnswerRepo userQuestionnaireAnswerRepo,
            ApplicationContext context)
        {
            _questionnaireRepo = questionnaireRepo;
            _questionRepo = questionRepo;
            _userRepo = userRepo;
            _patientQuestionnaireRepo = patientQuestionnaireRepo;
            _userManager = userManager;
            _userAnswerRepo = userAnswerRepo;
            _userQuestionnaireAnswerRepo = userQuestionnaireAnswerRepo;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var email = GetCurrentUserEmail();
            var currentUser = await _userRepo.GetUserByEmailAsync(email);

            var patients = await _userRepo.GetAllForCompany((int)currentUser.CompanyId);
            var model = (from patient in patients
                         where _userManager.IsInRoleAsync(patient, "Patient").Result
                         select new PatientViewModel
                         {
                             EmailAddress = patient.Email,
                             FirstName = patient.FirstName,
                             LastName = patient.LastName,
                         }).ToList();

            return View(model);
        }


        public async Task<ActionResult> PatientQuestionnaires(string id)
        {
            var patientQuestionnaires = await _patientQuestionnaireRepo.GetUserQuestionnairesByEmail(id);
            var patientQuestionnaireViewModels = patientQuestionnaires.Select(patientQuestionnaire =>
                new PatientQuestionnaireViewModel
                {
                    PatientEmail = id,
                    QuestionnaireId = patientQuestionnaire.QuestionnaireId,
                    QuestionnaireName = _questionnaireRepo.GetById(patientQuestionnaire.QuestionnaireId).Result.Name
                }).ToList();

            var model = new PatientQuestionnaireManagerViewModel
            {
                PatientEmail = id,
                PatientQuestionnaires = patientQuestionnaireViewModels
            };

            return View(model);
        }

        public async Task<ActionResult> JobOfferQuestionnaireResults(int Id)
        {
            var questionaire = (await _context
                .JobOffers
                .Include(q => q.Questionnaire)
                .ThenInclude(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .Include(q => q.Questionnaire)
                .ThenInclude(q => q.UserQuestionnaireAnswers)
                .ThenInclude(q => q.UserAnswers)
                .FirstOrDefaultAsync(q => q.Id == Id)
                )?.Questionnaire;

            var users = await _context.UserAccount
                .Where(u => questionaire
                            .UserQuestionnaireAnswers
                            .Select(q => q.UserEmail)
                            .ToList().Contains(u.Email))
                .ToListAsync();



            var questionnaireAnswerDetails = questionaire.UserQuestionnaireAnswers.Select(q => new QuestionnaireAnswerDetails
            {
                CandidateEmail = q.UserEmail,
                AnswerDate = q.AnswerDate,
                AnswerCount = q.UserAnswers.Count(),
                AnswerSum = q.UserAnswers.Where(u => u.Value).Count()
            }).ToList();

            questionnaireAnswerDetails = questionnaireAnswerDetails.Join(
                users,
                q => q.CandidateEmail,
                u => u.Email,
                (q, u) =>
                {
                    q.CandidateName = u.FirstName + " " + u.LastName;
                    return q;
                }).OrderByDescending(q => q.AnswerSum).ToList();


            var model = new ResultViewModel
            {
                QuestionnaireName = questionaire.Name,
                NumberOfSolvedQuestionnaires = questionaire.UserQuestionnaireAnswers.Count,
                StudyStart = questionnaireAnswerDetails.OrderBy(q => q.AnswerDate).FirstOrDefault()?.AnswerDate,
                LastQuestionnaireDate = questionnaireAnswerDetails.OrderByDescending(q => q.AnswerDate).FirstOrDefault()?.AnswerDate,
                QuestionnaireAnswerDetails = questionnaireAnswerDetails,
                PatientEmail = "tmp",
                FirstName = "tmp",
                LastName = "tmp",
                PESEL = "tmp",

            };

            return View(model);
        }

    }
}
