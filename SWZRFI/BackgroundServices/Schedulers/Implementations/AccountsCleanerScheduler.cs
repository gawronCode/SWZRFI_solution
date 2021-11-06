using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWZRFI.BackgroundServices.Schedulers.Abstract;
using SWZRFI.BackgroundServices.Services.Abstract;
using SWZRFI.BackgroundServices.Services.Implementations;


namespace SWZRFI.BackgroundServices.Schedulers.Implementations
{
    public class AccountsCleanerScheduler : BaseScheduler
    {
        public AccountsCleanerScheduler(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override DateTime NextScheduledOperation => DateTime.Now.AddHours(1);

        public override Task BaseServiceHandler(IServiceProvider scopeServiceProvider)
        {
            var service =
                scopeServiceProvider.GetServices<IBaseScheduledService>().FirstOrDefault(s
                    => s.GetType() == typeof(AccountsCleaner));
            
            return service?.BaseProcess();
        }
    }
}
