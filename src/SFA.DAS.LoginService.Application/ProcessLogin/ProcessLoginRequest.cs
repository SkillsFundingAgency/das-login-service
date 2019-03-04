using MediatR;

namespace SFA.DAS.LoginService.Application.ProcessLogin
{
    public class ProcessLoginRequest : IRequest<ProcessLoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}