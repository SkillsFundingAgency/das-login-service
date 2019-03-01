using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Reinvite;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb
{
    public class ConfirmCodeController : Controller
    {
        private readonly IMediator _mediator;

        public ConfirmCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Invitations/ConfirmCode/{invitationId}")]
        public async Task<ActionResult> Get(Guid invitationId)
        {
            var invitationResponse = await _mediator.Send(new GetInvitationByIdRequest(invitationId));

            if (invitationResponse != null && !invitationResponse.IsUserCreated && invitationResponse.ValidUntil >= SystemTime.UtcNow())
            {
                var confirmCodeViewModel = new ConfirmCodeViewModel(invitationResponse.Id, "");
                return View("ConfirmCode", confirmCodeViewModel);
            }

            return View("InvitationExpired", new InvitationExpiredViewModel() { InvitationId = invitationId });
        }

        [HttpPost("/Invitations/ConfirmCode/{invitationId}")]
        public async Task<ActionResult> Post(ConfirmCodeViewModel confirmCodeViewModel)
        {
            if (string.IsNullOrWhiteSpace(confirmCodeViewModel.Code))
            {
                ModelState.AddModelError("Code", "Enter confirmation code");

                return View("ConfirmCode", confirmCodeViewModel);
            }

            var confirmCodeResponse = await _mediator.Send(new ConfirmCodeRequest(confirmCodeViewModel.InvitationId, confirmCodeViewModel.Code));
            if (confirmCodeResponse.IsValid)
            {
                return RedirectToAction("Get", "CreatePassword", new { id = confirmCodeViewModel.InvitationId });
            }

            ModelState.AddModelError("Code", "Code not valid");

            return View("ConfirmCode", confirmCodeViewModel);
        }

        [HttpPost("/Invitations/Reinvite/{invitationId}")]
        public async Task<IActionResult> Reinvite(Guid invitationId)
        {
            var result = await _mediator.Send(new ReinviteRequest() { InvitationId = invitationId });
            if (result.Invited == false)
            {
                return BadRequest();
            }
            return RedirectToAction("Reinvited", "ConfirmCode", new { result.InvitationId });
        }

        [HttpGet("/Invitations/Reinvite/{invitationId}")]
        public async Task<IActionResult> Reinvited(Guid invitationId)
        {
            var invitation = await _mediator.Send(new GetInvitationByIdRequest(invitationId), CancellationToken.None);

            return View("Reinvited", new ReinvitedViewModel() { Email = invitation.Email });
        }
    }
}