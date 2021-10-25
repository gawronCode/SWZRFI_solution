using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWZRFI.DAL.Models.IdentityModels;

namespace SWZRFI.DAL.Contexts
{
    public class ContextAccounts : IdentityDbContext
    {
        public ContextAccounts(DbContextOptions<ContextAccounts> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccount { get; set; }
        

    }
}