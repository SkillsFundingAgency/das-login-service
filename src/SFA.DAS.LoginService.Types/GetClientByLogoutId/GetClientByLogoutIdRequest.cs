using System;
using MediatR;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Types.GetClientByLogoutId
{
    public class GetClientByLogoutIdRequest : IRequest<Client>
    {
        public string LogoutId { get; set; }
    }
}