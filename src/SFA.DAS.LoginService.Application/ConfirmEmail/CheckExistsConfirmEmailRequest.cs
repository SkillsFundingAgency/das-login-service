using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class CheckExistsConfirmEmailRequest : IRequest<CheckExistsConfirmEmailResponse>
    {
        public string Email { get; set; }
    }
}