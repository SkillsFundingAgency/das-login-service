using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
  public class CreatePasswordViewModel : PasswordViewModel
  {
    public Guid InvitationId { get; set; }
  }
}