using SWZRFI.ConfigData.Models;

namespace SWZRFI.ConfigData
{
    public interface IConfigGetter
    {
        IdentityEmailOptions GetIdentityEmail();
    }
}