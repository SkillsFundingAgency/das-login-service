using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.BuildLogoutViewModel
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, LogoutResponse>
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly LoginContext _loginContext;
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutHandler(IIdentityServerInteractionService interactionService, LoginContext loginContext, IUserService userService, IEventService eventService, IHttpContextAccessor httpContextAccessor)
        {
            _interactionService = interactionService;
            _loginContext = loginContext;
            _userService = userService;
            _eventService = eventService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            LogoutResponse response = new LogoutResponse
            {
                LogoutId = null
            };

            var context = await _interactionService.GetLogoutContextAsync(request.LogoutId);
            if (context != null)
            {
                response.PostLogoutRedirectUri = context.PostLogoutRedirectUri;
                
                // for the context to contain SignOutIFrameUrl the FrontChannelLogoutUri must have a value
                // and FrontChannelLogoutSessionRequired = 0 for at least one client in the current SSO login session

                // only those clients which are 'declared themselves' i.e. an authorize request was made to Login Service
                // will be included when the SignOutIframeUrl is called and generated it's child iFrames; currently
                // a second client will not be included as the Shared cookie authentication is breaking this.
                response.SignOutIframeUrl = context.SignOutIFrameUrl;

                var client = await _loginContext.Clients.SingleOrDefaultAsync(
                    c => c.IdentityServerClientId == context.ClientId,
                    cancellationToken: cancellationToken);

                if (client != null)
                {
                    response.ClientName = client.ServiceDetails.ServiceName;
                    response.LogoutId = request.LogoutId;

                    if (_httpContextAccessor.HttpContext.User?.Identity.IsAuthenticated == true)
                    {
                        await _userService.SignOutUser();

                        var principal = _httpContextAccessor.HttpContext.User;

                        await _eventService.RaiseAsync(new UserLogoutSuccessEvent(principal.GetSubjectId(),
                            principal.GetDisplayName()));
                    }
                }
                else
                {
                    _loginContext.UserLogs.Add(new UserLog()
                    {
                        Id = GuidGenerator.NewGuid(),
                        Action = "Logout user",
                        Email = string.Empty,
                        Result = "Invalid logoutId client does not exist",
                        DateTime = SystemTime.UtcNow()
                    });
                }
            }
            else
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(),
                    Action = "Logout user",
                    Email = string.Empty,
                    Result = "Invalid logoutId",
                    DateTime = SystemTime.UtcNow()
                });
            }

            return response;
        }
    }
}