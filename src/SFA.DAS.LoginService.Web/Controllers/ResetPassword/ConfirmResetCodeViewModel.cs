using System;
using System.ComponentModel.DataAnnotations;
using IdentityServer4.Models;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ConfirmResetCodeViewModel
    {
        public Guid RequestId { get; set; }
        [Required(ErrorMessage = "Code is required")]
        [MinLength(8, ErrorMessage = "Code must be 8 characters in length")]
        [MaxLength(8, ErrorMessage = "Code must be 8 characters in length")]
        public string Code { get; set; }
        public Guid ClientId { get; set; }
    }
}