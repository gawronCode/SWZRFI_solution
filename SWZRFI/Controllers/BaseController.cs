using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace SWZRFI.Controllers
{
    public class BaseController : Controller
    {
        protected string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
