using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using MediatR;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.ProcessLogin
{
    public class ProcessLoginHandler : IRequestHandler<ProcessLoginRequest, ProcessLoginResponse>
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly LoginContext _loginContext;

        public ProcessLoginHandler(IUserService userService, IEventService eventService,
            IIdentityServerInteractionService interactionService, LoginContext loginContext)
        {
            _userService = userService;
            _eventService = eventService;
            _interactionService = interactionService;
            _loginContext = loginContext;
        }

        public async Task<ProcessLoginResponse> Handle(ProcessLoginRequest request, CancellationToken cancellationToken)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(request.ReturnUrl);
            if (context == null)
            {
                return new ProcessLoginResponse(){Message = "Invalid ReturnUrl"};
            }

            var user = await _userService.FindByUsername(request.Username);
            if(user == null)
            {
                return new ProcessLoginResponse() { Message = "Invalid credentials" };
            }

            var emailConfirmed = await _userService.UserHasConfirmedEmail(user);
            if(!emailConfirmed)
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(),
                    Action = "Login",
                    Email = request.Username,
                    DateTime = SystemTime.UtcNow(),
                    Result = "Login Invalid Email Not Confirmed"
                });
                await _loginContext.SaveChangesAsync(cancellationToken);

                // raise some kind of event mediatr here so that a confirmation email can be generated and sent to the user if they don't already have one
                await _eventService.RaiseAsync(new UserLoginFailedEmailNotConfirmedEvent(user.UserName, user.Id, user.UserName));

                return new ProcessLoginResponse() { Message = "Email not confirmed" };
            }
            
            var signInResult = await _userService.SignInUser(request.Username, request.Password, request.RememberLogin);

            if (!signInResult.Succeeded)
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(), 
                    Action = "Login", 
                    Email = request.Username,  
                    DateTime = SystemTime.UtcNow(),
                    Result = "Login Invalid"
                });
                await _loginContext.SaveChangesAsync(cancellationToken);
                
                return signInResult.IsLockedOut 
                    ? new ProcessLoginResponse(){Message = "User account is locked out"} 
                    : new ProcessLoginResponse(){Message = "Invalid credentials"};
            }

            await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));
            
            var userLog = new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "Login", 
                Email = request.Username,  
                DateTime = SystemTime.UtcNow(),
                Result = "Login OK"
            };
            _loginContext.UserLogs.Add(userLog);
            await _loginContext.SaveChangesAsync(cancellationToken);
            
            return new ProcessLoginResponse() {CredentialsValid = true};
        }
    }
}