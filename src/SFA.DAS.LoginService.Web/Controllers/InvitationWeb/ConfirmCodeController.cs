using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.GetInvitationById;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationWeb
{
    public class ConfirmCodeController : Controller
    {
        private readonly IMediator _mediator;

        public ConfirmCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Invitations/ConfirmCode/{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var invitationResponse = await _mediator.Send(new GetInvitationByIdRequest(id));
            return invitationResponse != null ? View("ConfirmCode", invitationResponse) : View("InvitationExpired");
        }
    }
}