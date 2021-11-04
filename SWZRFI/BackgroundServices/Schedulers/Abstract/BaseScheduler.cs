using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWZRFI.BackgroundServices.Services.Abstract;

namespace SWZRFI.BackgroundServices.Schedulers.Abstract
{

    public abstract class BaseScheduler : IHostedService
    {
        private DateTime _nextRun;
        private Task _executingTask;

        public abstract DateTime NextScheduledOperation { get; }
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly CancellationTokenSource _stoppingCts = new();

        protected BaseScheduler(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _nextRun = DateTime.Now.AddSeconds(5);
            _serviceScopeFactory = serviceScopeFactory;
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
                _nextRun = NextScheduledOperation;

            } while (true);
        }

        protected async Task Process()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await BaseServiceHandler(scope.ServiceProvider);
        }

        public abstract Task BaseServiceHandler(IServiceProvider scopeServiceProvider);

    }

}
