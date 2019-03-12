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
            var logoutContext = await _interactionService.GetLogoutContextAsync(request.LogoutId);

            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.IdentityServerClientId == logoutContext.ClientId);

            var response = new LogoutResponse
            {
                LogoutId = request.LogoutId,
                PostLogoutRedirectUri = logoutContext.PostLogoutRedirectUri,
                ClientName = client.ServiceDetails.ServiceName
            };

            if (_httpContextAccessor.HttpContext.User?.Identity.IsAuthenticated == true)
            {
                await _userService.SignOutUser();

                var principal = _httpContextAccessor.HttpContext.User; 
            
                await _eventService.RaiseAsync(new UserLogoutSuccessEvent(principal.GetSubjectId(), principal.GetDisplayName()));
            }
            
            return response;
        }
    }
}