using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DTO.ViewModels;
using SWZRFI.DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class QuestionnaireManagerController : Controller
    {

        private readonly IQuestionnaireRepo _questionnaireRepo;
        private readonly IQuestionRepo _questionRepo;
        private readonly IAnswerRepo _answerRepo;
        private readonly IPatientQuestionnaireRepo _patientQuestionnaireRepo;

        public QuestionnaireManagerController(IQuestionnaireRepo questionnaireRepo,
            IQuestionRepo questionRepo,
            IAnswerRepo answerRepo,
            IPatientQuestionnaireRepo patientQuestionnaireRepo)
        {
            _questionnaireRepo = questionnaireRepo;
            _questionRepo = questionRepo;
            _answerRepo = answerRepo;
            _patientQuestionnaireRepo = patientQuestionnaireRepo;
        }

        
        public async Task<ActionResult> Index()
        {
            var questionnaires = await _questionnaireRepo.GetAll();
            var model = questionnaires.Select(questionnaire => new QuestionnaireViewModel {Fields = null, Id = questionnaire.Id, Name = questionnaire.Name}).ToList();

            return View(model);
        }


        public async Task<ActionResult> ManageQuestions(int id)
        {
            
            var questionnaire = await _questionnaireRepo.GetById(id);
            var questions = await _questionRepo.GetQuestionsByQuestionnaireId(id);

            var model = new QuestionnaireViewModel
            {
                Id = id,
                Name = questionnaire.Name,
                Fields = new List<FieldViewModel>()
            };

            var i = 0;
            foreach (var question in questions)
            {
                var answers = await _answerRepo.GetAnswersByQuestionId(question.Id);
                var answerViewModels = answers.Select(answer => new AnswerViewModel {Content = answer.Content, Value = answer.Value}).ToList();

                model.Fields.Add(new FieldViewModel
                {
                    Count = i,
                    Question = question.Contents,
                    QuestionId = question.Id,
                    Answers = answerViewModels
                });
                i++;
            }
            
            return View(model);
        }
        
        
        public async Task<ActionResult> CreateQuestionnaire(string questionnaireName)
        {

            if (questionnaireName == string.Empty || questionnaireName is null) return RedirectToAction(nameof(Index));

            var newQuestionnaire = new Questionnaire
            {
                CreationDate = DateTime.Now,
                Name = questionnaireName
            };

            await _questionnaireRepo.Create(newQuestionnaire);
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> RemoveQuestionnaire(int id)
        {
            var questions = await _questionRepo.GetQuestionsByQuestionnaireId(id);
            
            foreach (var question in questions)
            {
                var answers = await _answerRepo.GetAnswersByQuestionId(question.Id);
                foreach (var answer in answers)
                {
                    await _answerRepo.Delete(answer);
                }

                await _questionRepo.Delete(question);
            }

            var patientsQuestionnaires = _patientQuestionnaireRepo.GetPatientQuestionnairesByQuestionnaireId(id).Result;

            foreach (var patientQuestionnaire in patientsQuestionnaires)
            {
                await _patientQuestionnaireRepo.Delete(patientQuestionnaire);
            }

            var questionnaire = await _questionnaireRepo.GetById(id); 
            await _questionnaireRepo.Delete(questionnaire);
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> AddQuestionToQuestionnaire(string questionContent, int questionnaireId,
                                                        string answer1, bool val1,
                                                        string answer2, bool val2,
                                                        string answer3, bool val3,
                                                        string answer4, bool val4)
        {
            
            if (string.IsNullOrEmpty(questionContent) ||
                string.IsNullOrEmpty(answer1) ||
                string.IsNullOrEmpty(answer2) ||
                string.IsNullOrEmpty(answer3) ||
                string.IsNullOrEmpty(answer4)) return RedirectToAction("ErrorInfo", new { id = questionnaireId });
            
            var newQuestion = new Question
            {
                Contents = questionContent,
                QuestionnaireId = questionnaireId
            };
            
            await _questionRepo.Create(newQuestion);

            await _answerRepo.Create(new Answer
            {
                Content = answer1,
                Value = val1,
                QuestionId = newQuestion.Id
            });
            await _answerRepo.Create(new Answer
            {
                Content = answer2,
                Value = val2,
                QuestionId = newQuestion.Id
            });
            await _answerRepo.Create(new Answer
            {
                Content = answer3,
                Value = val3,
                QuestionId = newQuestion.Id
            });
            await _answerRepo.Create(new Answer
            {
                Content = answer4,
                Value = val4,
                QuestionId = newQuestion.Id
            });

            return RedirectToAction("ManageQuestions", new{id=questionnaireId});
        }


        public ActionResult ErrorInfo(int id)
        {
            return View(id);
        }


        public async Task<ActionResult> RemoveQuestionFromQuestionnaire(int id)
        {
            var question = await _questionRepo.GetById(id);
            var questionnaireId = question.QuestionnaireId;
            var answers = await _answerRepo.GetAnswersByQuestionId(question.Id);
            foreach (var answer in answers)
            {
                await _answerRepo.Delete(answer);
            }
            await _questionRepo.Delete(question);
            return RedirectToAction("ManageQuestions", new { id = questionnaireId });
        }

    }
}
