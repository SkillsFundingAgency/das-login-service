using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class RequestConfirmEmailHandler : IRequestHandler<RequestConfirmEmailRequest>
    {
        private readonly IEmailService _emailService;
        private readonly ILoginConfig _loginConfig;
        private readonly LoginContext _loginContext;
        private readonly IUserService _userService;

        public RequestConfirmEmailHandler(IEmailService emailService,
            ILoginConfig loginConfig, LoginContext loginContext, IUserService userService)
        {
            _emailService = emailService;
            _loginConfig = loginConfig;
            _loginContext = loginContext;
            _userService = userService;
        }

        public async Task<Unit> Handle(RequestConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);
            
            var loginUser = await _userService.FindByEmail(request.Email);
            if (loginUser == null)
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(), 
                    Action = "Request confirm email link", 
                    Email = request.Email,  
                    DateTime = SystemTime.UtcNow(),
                    Result = "Email does not belong to account"
                });
                await _loginContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            if(await _userService.UserHasConfirmedEmail(loginUser))
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(),
                    Action = "Request confirm email link",
                    Email = request.Email,
                    DateTime = SystemTime.UtcNow(),
                    Result = "Email has already been confirmed"
                });
                await _loginContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            await ClearOutAnyPreviousStillValidRequests(request.Email);

            var identityToken = await _userService.GenerateConfirmEmailToken(loginUser);

            var confirmEmailRequest = await SaveConfirmEmailRequest(request, cancellationToken, identityToken);

            // TODO: this is currently the Id of the request in the database it should be either the token or encrypted - the password version will be
            var resetUri = new Uri(new Uri(_loginConfig.BaseUrl), $"ConfirmEmail/{request.ClientId}/{confirmEmailRequest.Id}");
            
            await _emailService.SendConfirmEmail(new ConfirmEmailViewModel()
            {
                Contact = loginUser.GivenName,
                EmailAddress = request.Email,
                LoginLink = resetUri.ToString(), 
                ServiceName = client.ServiceDetails.ServiceName, 
                ServiceTeam = client.ServiceDetails.ServiceTeam, 
                Subject = "Confirm email", 
                TemplateId = client.ServiceDetails.EmailTemplates.Single(t => t.Name == "ConfirmEmail").TemplateId 
            });
            
            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "Request confirm email link", 
                Email = request.Email,  
                DateTime = SystemTime.UtcNow(),
                Result = "Sent confirm email link email"
            });
            await _loginContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }

        private async Task ClearOutAnyPreviousStillValidRequests(string email)
        {
            var stillValidRequests = await _loginContext.ConfirmEmailRequests.Where(r => r.Email == email &&  r.ValidUntil > SystemTime.UtcNow() && r.IsComplete == false).ToListAsync();
            stillValidRequests.ForEach(r => r.ValidUntil = SystemTime.UtcNow().AddDays(-1));
            await _loginContext.SaveChangesAsync();
        }

        private async Task<ConfirmEmailRequest> SaveConfirmEmailRequest(RequestConfirmEmailRequest request, CancellationToken cancellationToken,
            string identityToken)
        {
            var confirmEmailRequest = new ConfirmEmailRequest()
            {
                ClientId = request.ClientId,
                IsComplete = false,
                ValidUntil = SystemTime.UtcNow().AddHours(_loginConfig.ConfirmEmailExpiryInHours),
                Email = request.Email,
                RequestedDate = SystemTime.UtcNow(),
                IdentityToken = identityToken
            };
            _loginContext.ConfirmEmailRequests.Add(confirmEmailRequest);
            await _loginContext.SaveChangesAsync(cancellationToken);
            return confirmEmailRequest;
        }
    }
}