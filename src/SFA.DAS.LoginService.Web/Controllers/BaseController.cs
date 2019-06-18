using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Types.GetClientByLogoutId;
using SFA.DAS.LoginService.Types.GetClientByReturnUrl;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;

        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task SetViewBagClientIdByReturnUrl(string returnUrl)
        {
            var client = await Mediator.Send(new GetClientByReturnUrlRequest { ReturnUrl = returnUrl });            
            SetViewBagClientId(client?.Id);
        }

        public async Task SetViewBagClientIdByLogoutId(string logoutId)
        {
            var client = await Mediator.Send(new GetClientByLogoutIdRequest { LogoutId = logoutId });
            SetViewBagClientId(client?.Id);
        }

        public void SetViewBagClientId(Guid? clientId)
        {
            ViewBag.ClientId = clientId;
        }
    }
}