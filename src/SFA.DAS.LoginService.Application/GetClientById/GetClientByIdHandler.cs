using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientById;

namespace SFA.DAS.LoginService.Application.GetClientById
{
    public class GetClientByIdHandler : IRequestHandler<GetClientByIdRequest, Client>
    {
        private readonly IClientService _clientService;

        public GetClientByIdHandler(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Client> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
        {
            return await _clientService.GetByClientId(request.ClientId, cancellationToken);
        }
    }
}