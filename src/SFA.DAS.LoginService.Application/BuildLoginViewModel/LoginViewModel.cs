using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Application.BuildLoginViewModel
{
  public class LoginViewModel
  {
    [Required(ErrorMessage = "Enter an email address")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Enter a password")]
    public string Password { get; set; }
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
    public bool AllowRememberLogin { get; set; }
    public bool EnableLocalLogin { get; set; }
    public string ServiceName { get; set; }
    public string ServiceSupportUrl { get; set; }
    public Guid ClientId { get; set; }
    public CreateAccountDetails CreateAccountDetails { get; set; }
  }

  public class CreateAccountDetails
  {
    public bool LocalSignUp { get; set; }
    public string CreateAccountUrl { get; set; }
    public string Purpose { get; set; }
    public bool ShowCreateAccountLink => LocalSignUp || !string.IsNullOrWhiteSpace(CreateAccountUrl);
  }
}