using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly LoginContext _loginContext;

        public ClientService(IIdentityServerInteractionService interaction, LoginContext loginContext)
        {
            _interaction = interaction;
            _loginContext = loginContext;
        }

        public async Task<Client> GetByReturnUrl(string returnUrl, CancellationToken cancellationToken)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.ClientId != null)
            {
                return await GetByIdentityServerClientId(context.ClientId, cancellationToken);
            }

            return null;
        }

        public async Task<Client> GetByLogoutId(string logoutId, CancellationToken cancellationToken)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ClientId != null)
            {
                return await GetByIdentityServerClientId(context.ClientId, cancellationToken);
            }

            return null;
        }

        public async Task<Client> GetByIdentityServerClientId(string clientId, CancellationToken cancellationToken)
        {
            return await _loginContext.Clients.SingleOrDefaultAsync(c => c.IdentityServerClientId == clientId, cancellationToken: cancellationToken);
        }

        public async Task<Client> GetByClientId(Guid clientId, CancellationToken cancellationToken)
        {
            return await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == clientId, cancellationToken: cancellationToken);
        }
    }
}