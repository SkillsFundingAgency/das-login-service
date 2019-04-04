using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ConfirmEmail
{
    public class RequestConfirmEmailController : Controller
    {
        private readonly IMediator _mediator;

        public RequestConfirmEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Get(string returnUrl, string email)
        {    
            await _mediator.Send(new RequestConfirmEmailRequest { Email = email, ReturnUrl = returnUrl });   
            return View("ConfirmEmailSent", new ConfirmEmailSentViewModel() { Email = email });
        }
    }
}