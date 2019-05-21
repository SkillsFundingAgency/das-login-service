using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Types.GetClientById;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb
{
    public class SignUpCompleteController : BaseController
    {
        public SignUpCompleteController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("/Invitations/SignUpComplete/{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var invitation = await Mediator.Send(new GetInvitationByIdRequest(id), CancellationToken.None);
            if (invitation == null)
            {
                return BadRequest();
            }
            
            var client = await Mediator.Send(new GetClientByIdRequest() {ClientId = invitation.ClientId});
            SetViewBagClientId(client?.Id);
            
            return View("SignUpComplete", new SignUpCompleteViewModel(){UserRedirectUri = invitation.UserRedirectUri, ServiceName = client?.ServiceDetails.ServiceName});
        }
    }
}