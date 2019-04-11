using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.BuildLoginViewModel
{
    public class BuildLoginViewModelHandler : IRequestHandler<BuildLoginViewModelRequest, LoginViewModel>
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;
        private readonly LoginContext _loginContext;

        public BuildLoginViewModelHandler(IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider, IClientStore clientStore, LoginContext loginContext)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
            _loginContext = loginContext;
        }
        
        public async Task<LoginViewModel> Handle(BuildLoginViewModelRequest request, CancellationToken cancellationToken)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.returnUrl);
           
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var identityServiceClient = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (identityServiceClient != null)
                {
                    allowLocal = identityServiceClient.EnableLocalLogin;
                }
            }
           
            var client = await _loginContext.Clients.SingleAsync(c => c.IdentityServerClientId == context.ClientId, cancellationToken: cancellationToken);

            var loginViewModel = new LoginViewModel
            {
                AllowRememberLogin = false,
                EnableLocalLogin = allowLocal,
                ReturnUrl = request.returnUrl,
                Username = context?.LoginHint,
                ServiceName = client.ServiceDetails.ServiceName,
                ServiceSupportUrl = client.ServiceDetails.SupportUrl,
                ClientId = client.Id,
                CreateAccountDetails = new CreateAccountDetails()
            };

            if (client.AllowLocalSignUp)
            {
                loginViewModel.CreateAccountDetails.LocalSignUp = true;
            }

            if (!string.IsNullOrWhiteSpace(client.ServiceDetails.CreateAccountUrl))
            {
                loginViewModel.CreateAccountDetails.CreateAccountUrl = client.ServiceDetails.CreateAccountUrl;
            }

            if (!string.IsNullOrWhiteSpace(client.ServiceDetails.CreateAccountPurpose))
            {
                loginViewModel.CreateAccountDetails.Purpose = client.ServiceDetails.CreateAccountPurpose;
            }

            return loginViewModel;
        }
    }
}