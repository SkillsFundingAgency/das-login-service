using System;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class CreateInvitationResponse
    {
        public string Message { get; set; }
        public bool Invited { get; set; }
        public Guid InvitationId { get; set; }
        public string ExistingUserId { get; set; }

        public Guid ClientId { get; set; }
        public string ServiceName { get; set; }
        public string LoginLink { get; set; }
    }
}