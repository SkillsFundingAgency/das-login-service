using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.GetClientByReturnUrl
{
    public class GetClientByReturnUrlHandler : IRequestHandler<GetClientByReturnUrlRequest, Client>
    {
        private readonly IClientService _clientService;
        
        public GetClientByReturnUrlHandler(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Client> Handle(GetClientByReturnUrlRequest request, CancellationToken cancellationToken)
        {
            return await _clientService.GetByReturnUrl(request.ReturnUrl, cancellationToken);
        }
    }
}