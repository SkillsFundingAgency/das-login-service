using System;

namespace SFA.DAS.LoginService.Data.Entities
{
    public class UserLog
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Email { get; set; }
        public string Action { get; set; }
        public string Result { get; set; }
        public string ExtraData { get; set; }
    }
}