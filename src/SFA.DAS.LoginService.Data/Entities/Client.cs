using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string IdentityServerClientId { get; set; }
        public ServiceDetails ServiceDetails { get; set; }
        public bool AllowInvitationSignUp { get; set; }
        public bool AllowLocalSignUp { get; set; }
    }
}