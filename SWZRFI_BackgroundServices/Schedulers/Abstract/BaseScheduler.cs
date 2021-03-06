using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWZRFI_BackgroundServices.Services.Abstract;

namespace SWZRFI_BackgroundServices.Schedulers.Abstract
{

    public abstract class BaseScheduler : IHostedService
    {
        private DateTime _nextRun;
        private Task _executingTask;

        public abstract DateTime NextScheduledBackup { get; }
        protected readonly BaseService BaseService;

        private readonly CancellationTokenSource _stoppingCts = new();

        protected BaseScheduler(BaseService baseService) : base()
        {
            _nextRun = DateTime.Now.AddSeconds(5);
            BaseService = baseService;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null) return;

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Task.Delay(5000, stoppingToken);
                if (DateTime.Now <= _nextRun) continue;
                
                await Process();
                _nextRun = NextScheduledBackup;

            } while (true);
        }

        protected async Task Process()
        {
            await BaseServiceHandler(BaseService);
        }

        public abstract Task BaseServiceHandler(BaseService baseService);

    }

}
