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
        public DbSet<Location> Locations { get; set; }
        public DbSet<SkillRequirement> SkillRequirements { get; set; }

        //Pozwala na obejście problemu zapętlenia EF jak encje mają specyficzne relacje
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }

    }
}