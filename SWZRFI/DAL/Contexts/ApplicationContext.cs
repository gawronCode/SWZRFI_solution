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
        public DbSet<CompanyUserAccount> CompanyUserAccounts { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<JobOfferLocation> JobOfferLocations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SkillRequirement> SkillRequirements { get; set; }
        

    }
}