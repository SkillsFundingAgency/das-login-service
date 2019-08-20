using System;
using Newtonsoft.Json;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsApi.ViewModels
{
    public class InvitationRequestViewModel
    {
        public string Email { get; set; }
        [JsonProperty("given_name")]
        public string GivenName { get; set; }
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
        public string SourceId { get; set; }
        public Uri Callback { get; set; }
        public Uri UserRedirect { get; set; }
        public string OrganisationName { get; set; }
        public string Inviter { get; set; }
        public string InviterEmail { get; set; }
    }
}