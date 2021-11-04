using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.BackgroundServices.Services.Abstract;
using SWZRFI.DAL.Repositories.Interfaces;

namespace SWZRFI.BackgroundServices.Services.Implementations
{
    public class AccountsCleaner : BaseScheduledService
    {

        private readonly IUserRepo _userRepo;

        public AccountsCleaner(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public override Task BaseProcess()
        {
            return TryRemoveUnverifiedAccounts();
        }

        private async Task TryRemoveUnverifiedAccounts()
        {
            try
            {
                await _userRepo.DeleteUnverifiedAccountsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("TODO - logger błędów");
            }
        }
    }
}
