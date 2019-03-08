using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ConfirmResetCodeRequest : IRequest<ConfirmResetCodeResponse>
    {
        public Guid RequestId { get; }
        public string Code { get; }

        public ConfirmResetCodeRequest(Guid requestId, string code)
        {
            RequestId = requestId;
            Code = code;
        }
    }
}