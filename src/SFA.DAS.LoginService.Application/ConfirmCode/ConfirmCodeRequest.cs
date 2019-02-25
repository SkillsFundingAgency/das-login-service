using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ConfirmCode
{
    public class ConfirmCodeRequest : IRequest<ConfirmCodeResponse>
    {
        public ConfirmCodeRequest(Guid invitationId, string code)
        {
            InvitationId = invitationId;
            Code = code;
        }

        public Guid InvitationId { get; set; }
        public string Code { get; set; }
    }
}