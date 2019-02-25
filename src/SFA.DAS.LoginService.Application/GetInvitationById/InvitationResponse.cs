using System;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.GetInvitationById
{
    public class InvitationResponse
    {
        public InvitationResponse(Invitation invitation)
        {
            Id = invitation.Id;
            Email = invitation.Email;
            FamilyName = invitation.FamilyName;
            GivenName = invitation.GivenName;
            SourceId = invitation.SourceId;
            ValidUntil = invitation.ValidUntil;
            UserRedirectUri = invitation.UserRedirectUri;
            CallbackUri = invitation.CallbackUri;
            Code = invitation.Code;
            CodeConfirmed = invitation.CodeConfirmed;
        }

        public bool CodeConfirmed { get; set; }

        public string Code { get; set; }

        public Uri CallbackUri { get; set; }

        public Uri UserRedirectUri { get; set; }

        public DateTime ValidUntil { get; set; }

        public string SourceId { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Email { get; set; }

        public Guid Id { get; set; }
    }
}