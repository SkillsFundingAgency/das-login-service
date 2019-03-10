using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.GetInvitationById;
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
            
            if (!invitation.CodeConfirmed)
            {
                return BadRequest("CodeConfirmed is false");
            }
            return View("CreatePassword", new CreatePasswordViewModel(){InvitationId = id, PasswordViewModel = new PasswordViewModel(){ConfirmPassword = "", Password = ""}});
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
            
            if (!invitation.CodeConfirmed)
            {
                return BadRequest("CodeConfirmed is false");
            }
            
            if (vm.PasswordViewModel.Password == vm.PasswordViewModel.ConfirmPassword)
            {
                var response = await _mediator.Send(new CreatePasswordRequest {InvitationId = vm.InvitationId, Password = vm.PasswordViewModel.Password});
                if (response.PasswordValid)
                {
                    return RedirectToAction("Get", "SignUpComplete", new {id = vm.InvitationId});
                }

                ModelState.AddModelError("Password", "Password does not meet minimum complexity requirements");

                return View("CreatePassword",
                    new CreatePasswordViewModel()
                    {
                        InvitationId = vm.InvitationId, 
                        PasswordViewModel = new PasswordViewModel
                        {
                            Password = vm.PasswordViewModel.Password, 
                            ConfirmPassword = vm.PasswordViewModel.ConfirmPassword
                        }
                    });
            }
            
            ModelState.AddModelError("Password", "Passwords should match");
            
            return View("CreatePassword", new CreatePasswordViewModel()
            {
                InvitationId = vm.InvitationId, 
                PasswordViewModel = new PasswordViewModel
                {
                    Password = vm.PasswordViewModel.Password, 
                    ConfirmPassword = vm.PasswordViewModel.ConfirmPassword
                }
            });
        }
    }
}