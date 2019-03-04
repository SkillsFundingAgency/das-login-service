using MediatR;

namespace SFA.DAS.LoginService.Application.BuildLogoutViewModel
{
    public class LogoutRequest : IRequest<LogoutResponse>
    {
        public string LogoutId { get; set; }
    }
}