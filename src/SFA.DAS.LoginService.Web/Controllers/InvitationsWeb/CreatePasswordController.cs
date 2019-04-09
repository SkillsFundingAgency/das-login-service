using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Reinvite;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb
{
    public class CreatePasswordController : Controller
    {
        private readonly IMediator _mediator;

        public CreatePasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Invitations/CreatePassword/{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var invitation = await _mediator.Send(new GetInvitationByIdRequest(id));
            if (invitation == null)
            {
                return BadRequest("Invitation does not exist");
            }

            if (invitation.ValidUntil < SystemTime.UtcNow())
            {
                return View("InvitationExpired", new InvitationExpiredViewModel(){InvitationId = id});
            }


            return View("CreatePassword", new CreatePasswordViewModel()
            {
                InvitationId = id,
                Username = invitation.Email,
                Password = "",
                ConfirmPassword = ""
            });
        }

        [HttpPost("/Invitations/CreatePassword/{id}")]
        public async Task<ActionResult> Post(CreatePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreatePassword", vm);
            }
            
            var invitation = await _mediator.Send(new GetInvitationByIdRequest(vm.InvitationId));
            if (invitation == null)
            {
                return BadRequest("Invitation does not exist");
            }
            
            if (vm.Password == vm.ConfirmPassword)
            {
                var response = await _mediator.Send(new CreatePasswordRequest {InvitationId = vm.InvitationId, Password = vm.Password});
                if (response.PasswordValid)
                {
                    return RedirectToAction("Get", "SignUpComplete", new {id = vm.InvitationId});
                }

                ModelState.AddModelError("Password", "Password does not meet minimum complexity requirements");

                return View("CreatePassword",
                    new CreatePasswordViewModel()
                    {
                        InvitationId = vm.InvitationId, 
                        Password = vm.Password, 
                        ConfirmPassword = vm.ConfirmPassword
                    });
            }
            
            ModelState.AddModelError("Password", "Passwords should match");
            
            return View("CreatePassword", new CreatePasswordViewModel()
            {
                InvitationId = vm.InvitationId, 
                Password = vm.Password, 
                ConfirmPassword = vm.ConfirmPassword
            });
        }
        
        [HttpPost("/Invitations/Reinvite/{invitationId}")]
        public async Task<IActionResult> Reinvite(Guid invitationId)
        {
            var result = await _mediator.Send(new ReinviteRequest() { InvitationId = invitationId });
            if (result.Invited == false)
            {
                return BadRequest();
            }
            return RedirectToAction("Reinvited", new { result.InvitationId });
        }

        [HttpGet("/Invitations/Reinvite/{invitationId}")]
        public async Task<IActionResult> Reinvited(Guid invitationId)
        {
            var invitation = await _mediator.Send(new GetInvitationByIdRequest(invitationId), CancellationToken.None);

            return View("Reinvited", new ReinvitedViewModel() { Email = invitation.Email });
        }
    }
}