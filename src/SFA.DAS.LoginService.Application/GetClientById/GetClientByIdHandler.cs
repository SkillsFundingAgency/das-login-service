using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.GetClientById
{
    public class GetClientByIdHandler : IRequestHandler<GetClientByIdRequest, Client>
    {
        private readonly LoginContext _loginContext;

        public GetClientByIdHandler(LoginContext loginContext)
        {
            _loginContext = loginContext;
        }

        public async Task<Client> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
        {
            return await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken: cancellationToken);
        }
    }
}