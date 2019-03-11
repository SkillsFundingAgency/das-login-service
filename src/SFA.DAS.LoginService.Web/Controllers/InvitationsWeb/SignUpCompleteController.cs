using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb
{
    public class SignUpCompleteController : Controller
    {
        private readonly IMediator _mediator;

        public SignUpCompleteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Invitations/SignUpComplete/{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var invitation = await _mediator.Send(new GetInvitationByIdRequest(id), CancellationToken.None);
            if (invitation == null)
            {
                return BadRequest();
            }
            
            var client = await _mediator.Send(new GetClientByIdRequest() {ClientId = invitation.ClientId});
            
            return View("SignUpComplete", new SignUpCompleteViewModel(){UserRedirectUri = invitation.UserRedirectUri, ServiceName = client.ServiceDetails.ServiceName});
        }
    }
}