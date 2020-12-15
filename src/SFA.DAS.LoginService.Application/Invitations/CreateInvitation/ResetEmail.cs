using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class ResetEmailRequest : IRequest<ResetEmailResponse>
    {
        public string Email { get; set; }
        public Guid ClientId { get; set; }
        public Guid UserId { get; set; }
    }

    public class ResetEmailResponse
    {
        public bool ChangedSuccessfully { get; set; }
        public string Message { get; set; }
        public Guid ClientId { get; set; }
        public Guid ExistingUserId { get; set; }
    }

    public class ResetEmailHandler : IRequestHandler<ResetEmailRequest, ResetEmailResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly IEmailService _emailService;
        private readonly ILoginConfig _loginConfig;
        private readonly IUserService _userService;
        private readonly ILogger<ResetEmailHandler> _logger;

        public ResetEmailHandler(
            LoginContext loginContext,
            IEmailService emailService,
            ILoginConfig loginConfig,
            IUserService userService,
            ILogger<ResetEmailHandler> logger)
        {
            _loginContext = loginContext;
            _emailService = emailService;
            _loginConfig = loginConfig;
            _userService = userService;
            _logger = logger;
        }

        public async Task<ResetEmailResponse> Handle(ResetEmailRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"ResetEmailHandler : Create Invitation call received: {JsonConvert.SerializeObject(request)}");

            //ValidateRequest(request);

            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken: cancellationToken);
            if (client == null)
                return new ResetEmailResponse() { Message = "Client does not exist", ClientId = request.ClientId};

            var existingUser = await _userService.FindById(request.UserId);
            if (existingUser == null)
                return new ResetEmailResponse() { Message = "User does not exist", ClientId = request.ClientId, ExistingUserId = request.UserId};

            var r = await _userService.ChangeEmail(request.UserId, request.Email);

            return new ResetEmailResponse
            {
                ChangedSuccessfully = r.Result.Succeeded,
                Message = r.Result.Succeeded ? "User email changed" : "Failed to change email",
                ClientId = request.ClientId,
                ExistingUserId = request.UserId,
            };
        }
    }
}