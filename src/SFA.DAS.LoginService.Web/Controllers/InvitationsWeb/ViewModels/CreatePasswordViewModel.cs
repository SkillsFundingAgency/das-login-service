using System;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
  public class CreatePasswordViewModel : PasswordViewModel
  {
        public Guid InvitationId { get; set; }
  }
}