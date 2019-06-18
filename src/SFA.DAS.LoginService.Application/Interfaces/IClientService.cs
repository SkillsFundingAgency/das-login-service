using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> GetByReturnUrl(string returnUrl, CancellationToken cancellationToken);
        Task<Client> GetByLogoutId(string logoutId, CancellationToken cancellationToken);
        Task<Client> GetByClientId(Guid clientId, CancellationToken cancellationToken);
        Task<Client> GetByIdentityServerClientId(string clientId, CancellationToken cancellationToken);
    }
}