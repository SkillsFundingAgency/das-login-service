using System;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class ResetPasswordRequest
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Code { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsComplete { get; set; }
    }
}