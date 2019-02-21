using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class CreateInvitationHandler : IRequestHandler<CreateInvitationRequest, CreateInvitationResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly ICodeGenerationService _codeGenerationService;
        private readonly IHashingService _hashingService;
        private readonly IEmailService _emailService;
        private readonly ILoginConfig _loginConfig;

        public CreateInvitationHandler(LoginContext loginContext, ICodeGenerationService codeGenerationService,
            IHashingService hashingService, IEmailService emailService, ILoginConfig loginConfig)
        {
            _loginContext = loginContext;
            _codeGenerationService = codeGenerationService;
            _hashingService = hashingService;
            _emailService = emailService;
            _loginConfig = loginConfig;
        }

        public async Task<CreateInvitationResponse> Handle(CreateInvitationRequest request,
            CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var plainTextCode = _codeGenerationService.GenerateCode();
            var hashedCode = _hashingService.GetHash(plainTextCode);
            
            var newInvitation = new Invitation()
            {
                Email = request.Email,
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                SourceId = request.SourceId,
                Code = hashedCode,
                ValidUntil = SystemTime.UtcNow().AddHours(1),
                CallbackUri = request.Callback,
                UserRedirectUri = request.UserRedirect
            };
            
            _loginContext.Invitations.Add(newInvitation);
            await _loginContext.SaveChangesAsync(cancellationToken);

            var linkUrl = _loginConfig.BaseUrl + "Invitation/ConfirmCode/" + newInvitation.Id;

            await _emailService.SendInvitationEmail(request.Email, plainTextCode, linkUrl);
          
            return new CreateInvitationResponse();
        }

        private static void ValidateRequest(CreateInvitationRequest request)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(request.Email)) errors.Add("Email");
            if (string.IsNullOrWhiteSpace(request.GivenName)) errors.Add("GivenName");
            if (string.IsNullOrWhiteSpace(request.FamilyName)) errors.Add("FamilyName");
            if (string.IsNullOrWhiteSpace(request.SourceId)) errors.Add("SourceId");
            if (string.IsNullOrWhiteSpace(request.UserRedirect)) errors.Add("UserRedirect");
            if (request.Callback == null) errors.Add("Callback");

            if (errors.Any())
            {
                throw new ArgumentException(errors.ToArray().Join());
            }
        }
    }
}