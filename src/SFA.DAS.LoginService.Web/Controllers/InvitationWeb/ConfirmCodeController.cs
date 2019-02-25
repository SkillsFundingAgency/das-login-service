using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ConfirmCode;
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
            
            if (invitationResponse != null)
            {
                var confirmCodeViewModel = new ConfirmCodeViewModel(invitationResponse.Id, "");
                return View("ConfirmCode", confirmCodeViewModel);
            }
            
            return View("InvitationExpired");
        }

        [HttpPost("/Invitations/ConfirmCode/{id}")]
        public async Task<ActionResult> Post(ConfirmCodeViewModel confirmCodeViewModel)
        {
            var confirmCodeResponse = await _mediator.Send(confirmCodeViewModel);
            if (confirmCodeResponse.IsValid)
            {
                return RedirectToAction("Get", "CreatePassword");
            }

            ModelState.AddModelError("Code","Code not valid");
            
            return View("ConfirmCode", confirmCodeViewModel);
        }
    }
}