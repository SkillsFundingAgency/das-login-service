using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Application.GetClientByReturnUrl;

namespace SFA.DAS.LoginService.Web.Controllers.ConfirmEmail
{
    public class ConfirmEmailController : Controller
    {
        private readonly IMediator _mediator;

        public ConfirmEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// An email verification link has been clicked by a user to confirm their email address
        /// </summary>
        /// <param name="identityToken">The request identity token.</param>
        /// <returns></returns>
        [HttpGet("/ConfirmEmail/{returnUrl}/{identityToken}")]
        public async Task<IActionResult> Get(string returnUrl, string identityToken)
        {
            var decodedReturnUrl = Uri.UnescapeDataString(returnUrl);
            var response = await _mediator.Send(new VerifyConfirmEmailRequest { IdentityToken = Uri.UnescapeDataString(identityToken) });

            if (response.VerifyConfirmedEmailResult == VerifyConfirmedEmailResult.TokenVerified || 
                response.VerifyConfirmedEmailResult == VerifyConfirmedEmailResult.TokenPreviouslyVerified)
            {
                var client = await _mediator.Send(new GetClientByReturnUrlRequest() { ReturnUrl = decodedReturnUrl });
                var viewModel = new ConfirmEmailSuccessfulViewModel()
                {
                    ReturnUrl = decodedReturnUrl, // after confirming email return to the sign page which prompted the email confirmation
                    ServiceName = client.ServiceDetails.ServiceName
                };

                if (response.VerifyConfirmedEmailResult == VerifyConfirmedEmailResult.TokenVerified)
                {
                    return View("ConfirmEmailSuccessful", viewModel);
                }
                else
                {
                    return View("ConfirmEmailAlreadyConfirmed", viewModel);
                }
            }
            if (response.VerifyConfirmedEmailResult == VerifyConfirmedEmailResult.TokenExpired)
            { 
                return View("ConfirmEmailExpiredLink", new ConfirmEmailLinkViewModel() { Email = response.Email, ReturnUrl = decodedReturnUrl });
            }

            return View("ConfirmEmailInvalidLink", new ConfirmEmailLinkViewModel() { Email = response.Email, ReturnUrl = decodedReturnUrl });
        }

        /// <summary>
        /// A user has attempted to authenicate with the login service but their account email has not been verfied
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<IActionResult> EmailConfirmationRequired(string email, string returnUrl)
        {
            var checkExistsRequestResponse = await _mediator.Send(new CheckExistsConfirmEmailRequest { Email = email });
            if(checkExistsRequestResponse.HasRequest)
            {
                if(checkExistsRequestResponse.IsValid)
                {
                    // there is a request which has not expired, the user should reply to it or request a new email confirmation
                    return View("ConfirmEmailPendingLink", new ConfirmEmailLinkViewModel() { Email = email, ReturnUrl = returnUrl });
                }
                else
                {
                    // there is a request which has expired, the user should request a new email confirmation
                    return View("ConfirmEmailExpiredLink", new ConfirmEmailLinkViewModel() { Email = email, ReturnUrl = returnUrl });
                }
            }

            // there is no request a new email confirmation will need to be requested
            return View("ConfirmEmailRequired", new ConfirmEmailLinkViewModel() { Email = email, ReturnUrl = returnUrl });
        }
    }
}