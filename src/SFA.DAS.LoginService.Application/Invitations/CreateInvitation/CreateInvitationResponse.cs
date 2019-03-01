using System;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class CreateInvitationResponse
    {
        public string Message { get; set; }
        public bool Invited { get; set; }
        public Guid InvitationId { get; set; }
    }
}