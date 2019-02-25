using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationApi
{
    public class InvitationsController : Controller
    {
        private readonly IMediator _mediator;

        public InvitationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Invitations")]
        public async Task<ActionResult<CreateInvitationResponse>> Invite([FromBody] CreateInvitationRequest createInvitationRequest)
        {
            try
            {
               var response = await _mediator.Send(createInvitationRequest);
               return response;
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}