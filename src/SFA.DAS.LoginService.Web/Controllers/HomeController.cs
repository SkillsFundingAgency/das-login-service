using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("/TermsOfUse")]
        public IActionResult TermsOfUse(Guid clientId)
        {
            SetViewBagClientId(clientId);

            return View();
        }

        [HttpGet("/Privacy")]
        public IActionResult Privacy(Guid clientId)
        {
            SetViewBagClientId(clientId);

            return View();
        }

        [HttpGet("/Cookies")]
        public IActionResult Cookies(Guid clientId)
        {
            SetViewBagClientId(clientId);

            return View();
        }

        [HttpGet("/CookieDetails")]
        public IActionResult CookieDetails(Guid clientId)
        {
            SetViewBagClientId(clientId);

            return View();
        }

        [HttpGet("/Accessibility")]
        public IActionResult Accessibility(Guid clientId)
        {
            SetViewBagClientId(clientId);

            return View();
        }
    }
}