using System.Threading.Tasks;

namespace SWZRFI.BackgroundServices.Services.Abstract
{
    public abstract class BaseScheduledService : IBaseScheduledService
    {
        public abstract Task BaseProcess();
    }
}
