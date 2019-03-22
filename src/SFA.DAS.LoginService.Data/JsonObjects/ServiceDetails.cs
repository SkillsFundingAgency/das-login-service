using System.Collections.Generic;

namespace SFA.DAS.LoginService.Data.JsonObjects
{
    public class ServiceDetails
    {
        public string ServiceName { get; set; }
        public string ServiceTeam { get; set; }
        public string SupportUrl { get; set; }
        public string PostPasswordResetReturnUrl { get; set; }

        public List<EmailTemplate> EmailTemplates { get; set; }
        public string CreateAccountUrl { get; set; }
    }
}