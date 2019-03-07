using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ResetPassword
{
    public class When_POST_called_on_reset_password : ResetPasswordControllerTestBase
    {
        [Test]
        public async Task Then_Request_is_passed_on_to_mediator()
        {
            await Controller.Post(ClientId, new ResetPasswordViewModel{Email = "forgot@password.com"});
            await Mediator.Received().Send(Arg.Is<ResetPasswordCodeRequest>(r => r.Email == "forgot@password.com" && r.ClientId == ClientId));
        }

        [Test]
        public async Task Then_RedirectToAction_is_returned()
        {
            var result = await Controller.Post(ClientId, new ResetPasswordViewModel{Email = "forgot@password.com"});
            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).ActionName.Should().Be("CodeSent");
        }
    }
}