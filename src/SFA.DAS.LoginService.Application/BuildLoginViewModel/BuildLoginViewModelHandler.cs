using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace SFA.DAS.LoginService.Application.BuildLoginViewModel
{
    public class BuildLoginViewModelHandler : IRequestHandler<BuildLoginViewModelRequest, LoginViewModel>
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;

        public BuildLoginViewModelHandler(IIdentityServerInteractionService interaction, IAuthenticationSchemeProvider schemeProvider, IClientStore clientStore)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
        }
        
        public async Task<LoginViewModel> Handle(BuildLoginViewModelRequest request, CancellationToken cancellationToken)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.returnUrl);
            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = request.returnUrl,
                    Username = context?.LoginHint
                };
            }
            
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = false,
                EnableLocalLogin = allowLocal && true,
                ReturnUrl = request.returnUrl,
                Username = context?.LoginHint,
            };
        }
    }
}