using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ConfirmCode
{
    public class ConfirmCodeViewModel : IRequest<ConfirmCodeResponse>
    {
        public ConfirmCodeViewModel(){}
        
        public ConfirmCodeViewModel(Guid invitationId, string code)
        {
            InvitationId = invitationId;
            Code = code;
        }

        public Guid InvitationId { get; set; }
        public string Code { get; set; }
    }
}