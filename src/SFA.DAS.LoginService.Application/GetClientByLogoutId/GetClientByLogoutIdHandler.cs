using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientByLogoutId;

namespace SFA.DAS.LoginService.Application.GetClientByLogoutId
{
    public class GetClientByLogoutIdHandler : IRequestHandler<GetClientByLogoutIdRequest, Client>
    {
        private readonly IClientService _clientService;

        public GetClientByLogoutIdHandler(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Client> Handle(GetClientByLogoutIdRequest request, CancellationToken cancellationToken)
        {
            return await _clientService.GetByLogoutId(request.LogoutId, cancellationToken);
        }
    }
}