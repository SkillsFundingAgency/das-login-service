using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Web.Controllers.InvitationsApi.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsApi
{
    public class InvitationsController : Controller
    {
        private readonly IMediator _mediator;

        public InvitationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Invitations/{clientId}")]
        public async Task<ActionResult<CreateInvitationResponse>> Invite(Guid clientId, [FromBody] InvitationRequestViewModel createInvitationRequest)
        {
            try
            {
               var response = await _mediator.Send(new CreateInvitationRequest()
               {
                   Email = createInvitationRequest.Email,
                   GivenName = createInvitationRequest.GivenName,
                   FamilyName = createInvitationRequest.FamilyName,
                   SourceId = createInvitationRequest.SourceId,
                   Callback = createInvitationRequest.Callback,
                   UserRedirect = createInvitationRequest.UserRedirect,
                   ClientId = clientId
               });
               return response;
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}