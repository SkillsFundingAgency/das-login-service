using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ResetPasswordController : Controller
    {
        public async Task<IActionResult> Get(string clientid)
        {
            var vm = new ResetPasswordViewModel();
            return View("ResetPassword", vm);
        }
    }
}