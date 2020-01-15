using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Configuration;
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
        private readonly ILogger<CreateInvitationHandler> _logger;

        public CreateInvitationHandler(LoginContext loginContext, IEmailService emailService, ILoginConfig loginConfig,
            IUserService userService, ILogger<CreateInvitationHandler> logger)
        {
            _loginContext = loginContext;
            _emailService = emailService;
            _loginConfig = loginConfig;
            _userService = userService;
            _logger = logger;
        }

        public async Task<CreateInvitationResponse> Handle(CreateInvitationRequest request,
            CancellationToken cancellationToken)
        {
            
            _logger.LogInformation($"CreateInvitationHandler : Create Invitation call received: {JsonConvert.SerializeObject(request)}");
            
            ValidateRequest(request);

            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken: cancellationToken);
            if (client == null)
            {
                return new CreateInvitationResponse() {Message = "Client does not exist", ClientId = request.ClientId, Invited = false };
            }

            if (client.AllowInvitationSignUp == false)
            {
                return new CreateInvitationResponse() {Message = "Client is not authorised for Invitiation Signup", Invited = false, ClientId = request.ClientId, ServiceName = client.ServiceDetails?.ServiceName };
            }
            
            _logger.LogInformation($"CreateInvitationHandler : Client: {JsonConvert.SerializeObject(client)}");
            
            var existingUser = await _userService.FindByEmail(request.Email);
            if (existingUser != null)
            {
                await _emailService.SendUserExistsEmail(new UserExistsEmailViewModel
                {
                    Subject = "Sign up",
                    Contact = request.GivenName, 
                    LoginLink = client.ServiceDetails.PostPasswordResetReturnUrl, 
                    ServiceName = client.ServiceDetails.ServiceName, 
                    ServiceTeam = client.ServiceDetails.ServiceTeam, 
                    EmailAddress = request.Email,
                    TemplateId = client.ServiceDetails.EmailTemplates.Single(t => t.Name == "LoginSignupError").TemplateId
                });
                return new CreateInvitationResponse {Message = "User already exists", ExistingUserId = existingUser.Id, Invited = false, ClientId = request.ClientId, ServiceName = client.ServiceDetails?.ServiceName, LoginLink = client.ServiceDetails?.PostPasswordResetReturnUrl };
            }

            var inviteExists = _loginContext.Invitations.SingleOrDefault(i => i.Email == request.Email);
            if (inviteExists != null)
            {
                _loginContext.Invitations.Remove(inviteExists);
            }
            
            var newInvitation = new Invitation
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

            
            
            if (request.IsInvitationToOrganisation)
            {
                await _emailService.SendInvitationEmail(new InvitationEmailViewModel()
                {
                    Subject = "Sign up",
                    Contact = newInvitation.GivenName, 
                    LoginLink = linkUrl, 
                    ServiceName = client.ServiceDetails.ServiceName, 
                    ServiceTeam = client.ServiceDetails.ServiceTeam, 
                    EmailAddress = newInvitation.Email,
                    TemplateId = client.ServiceDetails.EmailTemplates.Single(t => t.Name == "LoginSignupInvite").TemplateId,
                    Inviter = $"{request.Inviter} of {request.OrganisationName}" 
                });
            }
            else
            {
                await _emailService.SendInvitationEmail(new InvitationEmailViewModel()
                {
                    Subject = "Sign up",
                    Contact = newInvitation.GivenName, 
                    LoginLink = linkUrl, 
                    ServiceName = client.ServiceDetails.ServiceName, 
                    ServiceTeam = client.ServiceDetails.ServiceTeam, 
                    EmailAddress = newInvitation.Email,
                    TemplateId = client.ServiceDetails.EmailTemplates.Single(t => t.Name == "SignUpInvitation").TemplateId,
                    Inviter = ""
                });
            }

            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "Invite", 
                Email = newInvitation.Email, 
                Result = "Invited", 
                DateTime = SystemTime.UtcNow()
            });
            
            await _loginContext.SaveChangesAsync(cancellationToken);
            return new CreateInvitationResponse(){Invited = true, InvitationId = newInvitation.Id, ClientId = request.ClientId, ServiceName = client.ServiceDetails?.ServiceName, LoginLink = client.ServiceDetails?.PostPasswordResetReturnUrl };
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