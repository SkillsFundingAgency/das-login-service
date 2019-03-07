using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Application.BuildLoginViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        public bool AllowRememberLogin { get; set; }
        public bool EnableLocalLogin { get; set; }
        public string ServiceName { get; set; }
        public string ServiceSupportUrl { get; set; }
        public Guid ClientId { get; set; }
    }
}