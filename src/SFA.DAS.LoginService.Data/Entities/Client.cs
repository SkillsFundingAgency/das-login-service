using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class Client
    {
        public string ServiceName { get; set; }
        public Guid Id { get; set; }
        public string IdentityServerClientId { get; set; }
    }
}