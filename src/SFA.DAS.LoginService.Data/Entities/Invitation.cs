using System;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string SourceId { get; set; }
        public string Code { get; set; }
        public DateTime ValidUntil { get; set; }
        public Uri CallbackUri { get; set; }
        public string UserRedirectUri { get; set; }
    }
}