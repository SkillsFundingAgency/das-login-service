using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmResetCode
{
    [TestFixture]
    public class When_GET_called_on_ConfirmResetCode_for_valid_link : ConfirmResetCodeTestBase
    {
        [Test]
        public async Task Then_correct_ViewResult_is_returned()
        {
            Mediator.Send(Arg.Any<CheckPasswordResetRequest>()).Returns(new CheckPasswordResetResponse(){IsValid = true});
            var result = await Controller.Get(ClientId, RequestId);

            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("ConfirmCode");
            result.As<ViewResult>().Model.Should().BeOfType<ConfirmResetCodeViewModel>();
            result.As<ViewResult>().Model.As<ConfirmResetCodeViewModel>().RequestId.Should().Be(RequestId);
            result.As<ViewResult>().Model.As<ConfirmResetCodeViewModel>().ClientId.Should().Be(ClientId);
        }
    }
}