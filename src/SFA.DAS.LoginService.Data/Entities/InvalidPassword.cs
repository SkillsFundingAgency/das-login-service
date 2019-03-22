using System;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class InvalidPassword
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
    }
}