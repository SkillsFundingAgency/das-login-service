using System.Linq;
using System.Web;
using System.Web.Mvc;

using SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework.Infrastructure;

namespace SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Application Description";

            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {
            ViewBag.Message = "Identity Claims";

            return View();
        }

        public ActionResult Signout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // call the signout of oidc to trigger the logout and post logout redirect of the login service
                HttpContext.GetOwinContext().Authentication.SignOut();

                // NOTE:: that the GetOwinContext() is an IIS specific extension, however since this is
                // a .Net Framework application it is almost certainly running on IIS at least on Windows
                // and if you wanted to use Linux or MacOS then the recommendation would be to create
                // a .Net Core application instead.
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AuthorizeExisting()
        {
            return RedirectToAction("SampleAction");
        }

        [CreateAccountAuthorize]
        public ActionResult AuthorizeNew()
        {
            // the Authorize attribute on this method will place the CreateAccount param in the context when
            // triggering a Sign-In - this will prompt the Login Service to enter the Sign-Up flow instead of
            // the Sign-In flow.
            return RedirectToAction("SampleAction");
        }

        [Authorize]
        public ActionResult SampleAction()
        {
            ViewBag.Message = "Sample Action";

            return View();
        }
    }
}