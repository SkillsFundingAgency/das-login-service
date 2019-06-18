using System;
using MediatR;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Types.GetClientByReturnUrl
{
    public class GetClientByReturnUrlRequest : IRequest<Client>
    {
        public string ReturnUrl { get; set; }
    }
}