using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Samples.MvcLocalClient.Infrastructure;
using SFA.DAS.LoginService.Samples.MvcLocalClient.Models;

namespace SFA.DAS.LoginService.Samples.MvcLocalClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Application Description";

            return View();
        }

        [Authorize]
        public IActionResult Claims()
        {
            ViewData["Message"] = "Identity Claims";

            return View();
        }

        public IActionResult Signout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // call the signout of oidc to trigger the logout and post logout redirect of the login service
                return SignOut("Cookies", "oidc");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult AuthorizeExisting()
        {
            return RedirectToAction("SampleAction");
        }

        [CreateAccountAuthorize]
        public IActionResult AuthorizeNew()
        {
            // the Authorize attribute on this method will place the CreateAccount param in the context when
            // triggering a Sign-In - this will prompt the Login Service to enter the Sign-Up flow instead of
            // the Sign-In flow.
            return RedirectToAction("SampleAction");
        }

        [Authorize]
        public IActionResult SampleAction()
        {
            ViewData["Message"] = "Sample Action";

            return View();
        }
    }
}
