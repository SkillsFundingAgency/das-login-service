using System;
using MediatR;
using Newtonsoft.Json;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class CreateInvitationRequest : IRequest<CreateInvitationResponse>
    {
        public string Email { get; set; }
        [JsonProperty("given_name")]
        public string GivenName { get; set; }
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
        public string SourceId { get; set; }
        public string UserRedirect { get; set; }
        public Uri Callback { get; set; }
    }
}