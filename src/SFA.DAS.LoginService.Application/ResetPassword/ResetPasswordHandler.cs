using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetUserPasswordRequest, ResetPasswordResponse>
    {
        private readonly IUserService _userService;
        private readonly LoginContext _loginContext;
        private readonly IEmailService _emailService;

        public ResetPasswordHandler(IUserService userService, LoginContext loginContext, IEmailService emailService)
        {
            _userService = userService;
            _loginContext = loginContext;
            _emailService = emailService;
        }

        public async Task<ResetPasswordResponse> Handle(ResetUserPasswordRequest request, CancellationToken cancellationToken)
        {
            var resetRequest = await _loginContext.ResetPasswordRequests.SingleOrDefaultAsync(r => r.Id == request.RequestId && r.IsComplete == false && r.ValidUntil > SystemTime.UtcNow(), cancellationToken: cancellationToken);
            var userResponse = await _userService.ResetPassword(resetRequest.Email, request.Password, resetRequest.IdentityToken);

            if (userResponse.Result != IdentityResult.Success)
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(), 
                    Action = "New password", 
                    Email = userResponse.User.Email,  
                    DateTime = SystemTime.UtcNow(),
                    Result = "Password invalid"
                });
                await _loginContext.SaveChangesAsync(cancellationToken);
                
                return new ResetPasswordResponse() {IsSuccessful = false};
            }

            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "New password", 
                Email = userResponse.User.Email,  
                DateTime = SystemTime.UtcNow(),
                Result = "Password changed"
            });
            
            resetRequest.IsComplete = true;
            await _loginContext.SaveChangesAsync(cancellationToken);

            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);
            var user = await _userService.FindByEmail(resetRequest.Email);
            
            await _emailService.SendPasswordReset(new PasswordResetEmailViewModel()
            {
                Subject = "Password has been reset",
                TemplateId = client.ServiceDetails.EmailTemplates.Single(t => t.Name == "LoginPasswordWasReset").TemplateId,
                Contact = user.GivenName,
                EmailAddress = resetRequest.Email,
                ServiceName = client.ServiceDetails.ServiceName,
                ServiceTeam = client.ServiceDetails.ServiceTeam,
                LoginLink = client.ServiceDetails.PostPasswordResetReturnUrl
            });            
            
            return new ResetPasswordResponse(){IsSuccessful = true, ClientId = request.ClientId, ReturnUrl = client.ServiceDetails.PostPasswordResetReturnUrl};
        }
    }
}