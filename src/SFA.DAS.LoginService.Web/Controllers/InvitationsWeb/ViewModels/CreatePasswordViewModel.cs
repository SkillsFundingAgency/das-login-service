using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
  public class CreatePasswordViewModel
  {
    public Guid InvitationId { get; set; }
    [Required(ErrorMessage = "Enter a password")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm your password")]
    public string ConfirmPassword { get; set; }
  }
}