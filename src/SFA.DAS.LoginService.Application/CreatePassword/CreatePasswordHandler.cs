using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.CreatePassword
{
    public class CreatePasswordHandler : IRequestHandler<CreatePasswordRequest, CreatePasswordResponse>
    {
        private readonly IUserService _userService;
        private readonly LoginContext _loginContext;
        private readonly ICallbackService _callbackService;

        public CreatePasswordHandler(IUserService userService, LoginContext loginContext, ICallbackService callbackService)
        {
            _userService = userService;
            _loginContext = loginContext;
            _callbackService = callbackService;
        }

        public async Task<CreatePasswordResponse> Handle(CreatePasswordRequest request, CancellationToken cancellationToken)
        {
            var invitation = await _loginContext.Invitations.SingleOrDefaultAsync(i => i.Id == request.InvitationId, cancellationToken: cancellationToken);
            var newUserResponse = await _userService.CreateUser(new LoginUser(){UserName = invitation.Email, Email = invitation.Email}, request.Password);

            if (newUserResponse.Result != IdentityResult.Success)
            {
                return new CreatePasswordResponse(){PasswordValid = false};    
            }
            
            invitation.IsUserCreated = true;

            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "Create password", 
                Email = invitation.Email, 
                Result = "User account created", 
                DateTime = SystemTime.UtcNow()
            });
            
            await _loginContext.SaveChangesAsync(cancellationToken);
            
            await _callbackService.Callback(invitation, newUserResponse.User.Id);
            
            return new CreatePasswordResponse(){PasswordValid = true};
        }
    }
}