using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Models.IdentityModels;

namespace SWZRFI.DAL.Contexts
{
    public class ContextEf : IdentityDbContext
    {
        public ContextEf(DbContextOptions<ContextEf> options)
            : base(options)
        {
        }

        public DbSet<PersonalAccount> PersonalAccounts { get; set; }
        public DbSet<CorporateAccount> CorporateAccounts { get; set; }

    }
}