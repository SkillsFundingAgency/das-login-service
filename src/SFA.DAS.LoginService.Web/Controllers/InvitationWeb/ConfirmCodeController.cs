using System;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationWeb
{
    public class ConfirmCodeController : Controller
    {
        public ActionResult Get(Guid newGuid)
        {
            return View("ConfirmCode");
        }
    }
}