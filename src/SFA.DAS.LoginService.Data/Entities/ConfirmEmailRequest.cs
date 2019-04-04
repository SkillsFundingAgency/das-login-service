using System;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class ConfirmEmailRequest
    {
        public Guid Id { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsComplete { get; set; }
        public string Email { get; set; }
        public DateTime RequestedDate { get; set; }
        public string IdentityToken { get; set; }
    }
}