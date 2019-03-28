using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ConfirmEmail
{
    public class ConfirmEmailController : Controller
    {
        private readonly IMediator _mediator;

        public ConfirmEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/ConfirmEmail/{clientId}/{requestId}")]
        public async Task<IActionResult> Get(Guid clientId, Guid requestId)
        {
            var checkRequestResponse = await _mediator.Send(new CheckConfirmEmailRequest {RequestId = requestId});
            if(checkRequestResponse.IsValid)
            {
                var client = await _mediator.Send(new GetClientByIdRequest() { ClientId = clientId });
                return View("ConfirmEmailSuccessful", new ConfirmEmailSuccessfulViewModel()
                {
                    ReturnUrl = client.ServiceDetails.PostPasswordResetReturnUrl,
                    ServiceName = client.ServiceDetails.ServiceName
                });
            }

            return View("ConfirmEmailExpiredLink", new ConfirmEmailExpiredLinkViewModel(){ClientId = clientId, Email = checkRequestResponse.Email });
        }
    }
}