using System;

namespace SFA.DAS.LoginService.Application.Services.EmailServiceViewModels
{
    public class EmailViewModel
    {
        public Guid TemplateId { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
    }
}