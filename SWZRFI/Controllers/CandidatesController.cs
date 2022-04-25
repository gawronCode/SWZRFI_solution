using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Contexts;
using SWZRFI.DTO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SWZRFI.DTO.DtoMappingUtils;
using SWZRFI.DAL.Models;
using SWZRFI_Utils.EmailHelper;
using SWZRFI.ConfigData;
using SWZRFI_Utils.EmailHelper.Models;
using System.Collections.Generic;
using System;

namespace SWZRFI.Controllers
{
    [Authorize(Roles = "SystemAdmin,RecruitersAccount,ManagerAccount")]
    public class CandidatesController : BaseController
    {
        private readonly ApplicationContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfigGetter _configGetter;


        public CandidatesController(ApplicationContext context,
                                    IEmailSender emailSender,
                                    IConfigGetter configGetter)
        {
            _context = context;
            _emailSender = emailSender;
            _configGetter = configGetter;
        }


        public async Task<IActionResult> Index()
        {
            var userEmail = GetCurrentUserEmail();
            var currentUser = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == userEmail);

            var company = await _context.Companies.Where(c => c.Id == currentUser.CompanyId).AsSplitQuery()
                .Include(c => c.JobOffers)
                .ThenInclude(j => j.Applications)
                .ThenInclude(a => a.UserAccount)
                .ThenInclude(u => u.Cv).ToListAsync();

            var questionnaireInvitations = await _context.QuestionnaireAccesses
                .ToListAsync();

            var userQuestionnaireAnswers = await _context.UserQuestionnaireAnswers
                .AsSplitQuery()
                .Include(q => q.Questionnaire)
                .ToListAsync();


            var jobOfferApplications = company
                .SelectMany(c => c.JobOffers)
                .Select(j => new JobOfferApplication
                {
                    JobOffer = j,
                    CandidateApplications = j.Applications.Select(a => new CandidateApplication
                    {
                        Application = a,
                        Cv = a.UserAccount.Cv,
                        UserAccount = a.UserAccount,
                        QuestionnaireInvitation = questionnaireInvitations
                        .Where(q => company.SelectMany(c => c.JobOffers).Select(j => j.Id).ToList().Contains(q.JobOfferId))
                        .Any(q => q.UserId == a.UserAccountId && q.JobOfferId == a.JobOfferId),
                        SolvedQuestionnaire = userQuestionnaireAnswers
                        .Where(uq => questionnaireInvitations.Select(q => q.QuestionnaireId).Contains(uq.QuestionnaireId))
                        .Any(q => q.UserEmail == a.UserAccount.Email && a.JobOffer.QuestionnaireId == q.QuestionnaireId)
                    }).ToList()
                }).ToList();



            var model = jobOfferApplications.GroupBy(j => j.JobOffer.Id).ToList();


            return View(jobOfferApplications);
        }

        private async Task SendEmail(string email, JobOffer jobOffer)
        {
            var identityEmail = _configGetter.GetIdentityEmail();
            var emailCredentials = new EmailCredentials
            {
                EmailAddress = identityEmail.Email,
                Password = identityEmail.Password,
                Port = identityEmail.Port,
                SmtpHost = identityEmail.Smtp
            };

            var emailMessage = new EmailMessage
            {
                Recipients = new List<string>() { email },
                Subject = $"Zakończenie procesu rekrutacji - {jobOffer.Title}",
                Content =
                    $"Twoja aplikacja na ofertę \"{jobOffer.Title}\" " +
                    $"nie zakwalifikowała się do kolejnego etapu procesu rekrutacyjnego. " +
                    $"Dziękujemy za udział w rekrutacji i życzymy sukcesów w przyszłości."
            };


            await _emailSender.Send(emailCredentials, emailMessage);
        }

        private async Task SendInvitationEmail(string email, JobOfferUserMeeting meetingInvitation)
        {
            var identityEmail = _configGetter.GetIdentityEmail();
            var emailCredentials = new EmailCredentials
            {
                EmailAddress = identityEmail.Email,
                Password = identityEmail.Password,
                Port = identityEmail.Port,
                SmtpHost = identityEmail.Smtp
            };

            var emailMessage = new EmailMessage
            {
                Recipients = new List<string>() { email },
                Subject = $"Zaproszenie na spotkanie w związku z apliakcją na ofertę {meetingInvitation.JobOffer.Title}",
                Content =
                    $"Otrzymano zaproszenie na rozmowę w związu z aplikacją na ofertę  \"{meetingInvitation.JobOffer.Title}\" " +
                    $"Spotkanie zaplanowano na {meetingInvitation.MeetingDateTime.ToString("yyyy-MM-dd hh:mm")}." +
                    $"W celu potwierdzenia uczestnictwa, zaloguj się do swojego konta kandydata i przejdź do zakładki \"Aplikacje\". " +
                    $"W przypdku niedogodności związanych z terminem, skontaktuj się ze swoim rekruterem pod adresem email: {meetingInvitation.UserAccount.Email}."
            };


            await _emailSender.Send(emailCredentials, emailMessage);
        }


        public async Task<IActionResult> DeleteApplication(int Id)
        {
            var application = await _context.Applications
                .AsSplitQuery()
                .Include(a => a.UserAccount)
                .Include(a => a.JobOffer)
                .FirstOrDefaultAsync(q => q.Id == Id);
            var user = application.UserAccount;
            var jobOffer = application.JobOffer;

            var questionnaireAnswer = await _context.UserQuestionnaireAnswers.Include(q => q.UserAnswers)
                .FirstAsync(q => q.QuestionnaireId == jobOffer.QuestionnaireId && q.UserEmail == user.Email);

            _context.RemoveRange(questionnaireAnswer.UserAnswers);
            await _context.SaveChangesAsync();
            _context.Remove(questionnaireAnswer);
            await _context.SaveChangesAsync();

            _context.Remove(application);
            await _context.SaveChangesAsync();

            await SendEmail(user.Email, jobOffer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> InviteToMeeting(int id)
        {
            var application = await _context.Applications
                .AsSplitQuery()
                .Include(q => q.JobOffer)
                .ThenInclude(q => q.Questionnaire)
                .Include(q => q.Cv)
                .Include(q => q.UserAccount)
                .FirstOrDefaultAsync(q => q.Id == id);

            var user = await _context.UserAccount.FirstOrDefaultAsync(q => q.Email == GetCurrentUserEmail());



            var meetingInvitation = new JobOfferUserMeeting
            {
                Application = application,
                ApplicationId = application.Id,
                JobOfferId = application.JobOfferId,
                UserAccount = user,
                UserAccountId = user.Id,
                MeetingDateTime = DateTime.Today.AddDays(7)
            };

            return View(meetingInvitation);
        }

        public async Task<IActionResult> SendInvitationForMeeting(JobOfferUserMeeting meetingInvitation)
        {
            var application = await _context.Applications.AsSplitQuery()
                .Include(q => q.UserAccount)
                .FirstOrDefaultAsync(q => q.UserAccountId == meetingInvitation.UserAccountId && q.JobOfferId == meetingInvitation.JobOfferId);

            var candidate = application.UserAccount;

            _context.JobOfferUserMeetings.Add(meetingInvitation);
            await _context.SaveChangesAsync();

            var invitation = await _context.JobOfferUserMeetings.AsSplitQuery()
                .Include(q => q.UserAccount)
                .Include(q => q.Application)
                .ThenInclude(q => q.UserAccount)
                .Include(q => q.Application)
                .ThenInclude(q => q.JobOffer)
                .FirstOrDefaultAsync(q => q.Id == meetingInvitation.Id);

            await SendInvitationEmail(invitation.Application.UserAccount.Email, invitation);

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> CandidateApplication(int id)
        {
            var application = await _context.Applications
                .AsSplitQuery()
                .Include(q => q.JobOffer)
                .ThenInclude(q => q.Questionnaire)
                .Include(q => q.Cv)
                .Include(q => q.UserAccount)
                .FirstOrDefaultAsync(q => q.Id == id);


            var questionnaireAccess = await _context
                .QuestionnaireAccesses
                .FirstOrDefaultAsync(q => q.UserId == application.UserAccountId && q.JobOfferId == application.JobOfferId);

            var userAnswer = await _context
                .UserQuestionnaireAnswers
                .AsSplitQuery()
                .Include(q => q.UserAnswers)
                .FirstOrDefaultAsync(q => q.QuestionnaireId == application.JobOffer.QuestionnaireId);


            var cvViewModel = application.Cv.MapToDto();
            cvViewModel.ApplicationId = id;
            cvViewModel.Email = application.UserAccount.Email;
            cvViewModel.JobOfferId = application.JobOfferId;
            cvViewModel.UserId = application.UserAccountId;


            var applicationViewer = new ApplicationViewer
            {
                CvViewer = cvViewModel,
                JobOffer = application.JobOffer,
                UserQuestionnaireAnswer = userAnswer,
                QuestionnaireAccess = questionnaireAccess
            };


            application.Opened = true;
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();


            return View(applicationViewer);
        }

        public async Task<IActionResult> SendQuestionnaireInvitation(string id)
        {
            var data = id.Split(',');
            var user = await _context.UserAccount.FirstOrDefaultAsync(q => q.Id == data[0]);
            var jobOffer = await _context.JobOffers.Include(q => q.Questionnaire).FirstOrDefaultAsync(q => q.Id == int.Parse(data[1]));

            if(jobOffer.QuestionnaireId == null)
                return RedirectToAction(nameof(Index));


            var invitation = new QuestionnaireAccess
            {
                Expired = false,
                QuestionnaireId = (int)jobOffer.QuestionnaireId,
                JobOfferId = jobOffer.Id,
                QuestionnaireTitle = jobOffer.Questionnaire.Name,
                UserId = user.Id
            };

            _context.QuestionnaireAccesses.Add(invitation);
            _context.UserQuestionnaires.Add(new UserQuestionnaire 
            { 
                PatientEmail = user.Email, 
                QuestionnaireId = jobOffer.Questionnaire.Id 
            });


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

    }
}
