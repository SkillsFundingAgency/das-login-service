using System;

namespace SFA.DAS.LoginService.Application.Services
{
    public class InvitationEmailViewModel : EmailViewModel
    {
        public string Contact { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTeamName { get; set; }
        public string LoginLink { get; set; }
    }
    
    public class PasswordResetEmailViewModel : EmailViewModel
    {
        public string Contact { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTeamName { get; set; }
        public string LoginLink { get; set; }
    }
    
    public class PasswordResetNoAccountEmailViewModel : EmailViewModel
    {
        public string ServiceName { get; set; }
        public string ServiceTeamName { get; set; }
        public string LoginLink { get; set; }
    }

    public class EmailViewModel
    {
        public Guid TemplateId { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
    }
}