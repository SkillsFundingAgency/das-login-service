using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using MediatR;
using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.ProcessLogin
{
    public class ProcessLoginHandler : IRequestHandler<ProcessLoginRequest, ProcessLoginResponse>
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly IIdentityServerInteractionService _interactionService;

        public ProcessLoginHandler(IUserService userService, IEventService eventService,
            IIdentityServerInteractionService interactionService)
        {
            _userService = userService;
            _eventService = eventService;
            _interactionService = interactionService;
        }

        public async Task<ProcessLoginResponse> Handle(ProcessLoginRequest request, CancellationToken cancellationToken)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);
            if (context == null)
            {
                return new ProcessLoginResponse(){Message = "Invalid ReturnUrl"};
            }
            
            var signInResult = await _userService.SignInUser(request.Username, request.Password, request.RememberLogin);

            if (!signInResult.Succeeded)
            {
                return signInResult.IsLockedOut 
                    ? new ProcessLoginResponse(){Message = "User account is locked out"} 
                    : new ProcessLoginResponse(){Message = "Invalid credentials"};
            }

            var user = await _userService.FindByUsername(request.Username);

            await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));
            
            return new ProcessLoginResponse() {CredentialsValid = true};
        }
    }
}