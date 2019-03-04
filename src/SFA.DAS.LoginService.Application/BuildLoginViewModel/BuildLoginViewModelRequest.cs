using MediatR;

namespace SFA.DAS.LoginService.Application.BuildLoginViewModel
{
    public class BuildLoginViewModelRequest : IRequest<LoginViewModel>
    {
        public string returnUrl { get; set; }
    }
}