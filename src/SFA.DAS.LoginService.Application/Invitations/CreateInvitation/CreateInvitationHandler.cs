using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.Configuration;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class CreateInvitationHandler : IRequestHandler<CreateInvitationRequest, CreateInvitationResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly IEmailService _emailService;
        private readonly ILoginConfig _loginConfig;
        private readonly IUserService _userService;

        public CreateInvitationHandler(LoginContext loginContext, IEmailService emailService, ILoginConfig loginConfig,
            IUserService userService)
        {
            _loginContext = loginContext;
            _emailService = emailService;
            _loginConfig = loginConfig;
            _userService = userService;
        }

        public async Task<CreateInvitationResponse> Handle(CreateInvitationRequest request,
            CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken: cancellationToken);
            if (client == null)
            {
                return new CreateInvitationResponse() {Message = "Client does not exist"};
            }

            if (client.AllowInvitationSignUp == false)
            {
                return new CreateInvitationResponse() {Message = "Client is not authorised for Invitiation Signup"};
            }
            
            var userExists = await _userService.UserExists(request.Email);
            if (userExists)
            {
                return new CreateInvitationResponse() {Message = "User already exists"};
            }

            var inviteExists = _loginContext.Invitations.SingleOrDefault(i => i.Email == request.Email);
            if (inviteExists != null)
            {
                _loginContext.Invitations.Remove(inviteExists);
            }
            
            var newInvitation = new Invitation()
            {
                Email = request.Email,
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                SourceId = request.SourceId,
                ValidUntil = SystemTime.UtcNow().AddHours(1),
                CallbackUri = request.Callback,
                UserRedirectUri = request.UserRedirect,
                ClientId = request.ClientId
            };
            
            _loginContext.Invitations.Add(newInvitation);

            var linkUri = new Uri(_loginConfig.BaseUrl);
            var linkUrl = new Uri(linkUri, "Invitations/CreatePassword/" + newInvitation.Id).ToString();

            await _emailService.SendInvitationEmail(new InvitationEmailViewModel()
            {
                Subject = "Sign up",
                Contact = newInvitation.GivenName, 
                LoginLink = linkUrl, 
                ServiceName = client.ServiceDetails.ServiceName, 
                ServiceTeam = client.ServiceDetails.ServiceTeam, 
                EmailAddress = newInvitation.Email,
                TemplateId = client.ServiceDetails.EmailTemplates.Single(t => t.Name == "SignUpInvitation").TemplateId
            });

            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "Invite", 
                Email = newInvitation.Email, 
                Result = "Invited", 
                DateTime = SystemTime.UtcNow()
            });
            
            await _loginContext.SaveChangesAsync(cancellationToken);
            return new CreateInvitationResponse(){Invited = true, InvitationId = newInvitation.Id};
        }

        private static void ValidateRequest(CreateInvitationRequest request)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(request.Email)) errors.Add("Email");
            if (string.IsNullOrWhiteSpace(request.GivenName)) errors.Add("GivenName");
            if (string.IsNullOrWhiteSpace(request.FamilyName)) errors.Add("FamilyName");
            if (string.IsNullOrWhiteSpace(request.SourceId)) errors.Add("SourceId");
            if (request.UserRedirect == null) errors.Add("UserRedirect");
            if (request.Callback == null) errors.Add("Callback");

            if (errors.Any())
            {
                throw new ArgumentException(errors.ToArray().Join());
            }
        }
    }
}