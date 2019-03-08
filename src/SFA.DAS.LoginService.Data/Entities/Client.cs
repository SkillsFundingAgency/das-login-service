using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string IdentityServerClientId { get; set; }
        
        public ServiceDetails ServiceDetails { get; set; }
    }

    public class ServiceDetails
    {
        public string ServiceName { get; set; }
        public string SupportUrl { get; set; }
        public string PostPasswordResetReturnUrl { get; set; }
    }
}