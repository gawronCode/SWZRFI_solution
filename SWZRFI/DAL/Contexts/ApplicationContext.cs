using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Models;

namespace SWZRFI.DAL.Contexts
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<CorporationalInvitation> CorporationalInvitations { get; set; }
        public DbSet<Cv> Cvs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        public DbSet<QuestionnaireAccess> QuestionnaireAccesses { get; set; }

        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserQuestionnaire> UserQuestionnaires { get; set; }
        public DbSet<UserQuestionnaireAnswer> UserQuestionnaireAnswers { get; set; }
        public DbSet<JobOfferUserMeeting> JobOfferUserMeetings { get; set; }


        //Pozwala na obejście problemu zapętlenia EF jak encje mają specyficzne relacje
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {



            modelbuilder.Entity<Question>().HasOne(q => q.Questionnaire).WithMany(q => q.Questions);
            modelbuilder.Entity<Answer>().HasOne(q => q.Question).WithMany(q => q.Answers);
            modelbuilder.Entity<UserQuestionnaireAnswer>().HasOne(q => q.Questionnaire).WithMany(q => q.UserQuestionnaireAnswers);
            modelbuilder.Entity<UserAnswer>().HasOne(q => q.UserQuestionnaireAnswer).WithMany(q => q.UserAnswers);

            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelbuilder);
        }

    }
}