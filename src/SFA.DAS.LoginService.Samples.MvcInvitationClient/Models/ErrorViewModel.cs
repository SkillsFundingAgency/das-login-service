using System;

namespace SFA.DAS.LoginService.Samples.MvcInvitationClient.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}