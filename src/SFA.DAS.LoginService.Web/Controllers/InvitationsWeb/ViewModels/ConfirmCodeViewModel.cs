using System;
using MediatR;
using SFA.DAS.LoginService.Application.ConfirmCode;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
    public class ConfirmCodeViewModel 
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