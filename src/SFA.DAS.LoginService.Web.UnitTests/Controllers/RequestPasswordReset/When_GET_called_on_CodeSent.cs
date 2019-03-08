using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ResetPassword
{
    public class When_GET_called_on_CodeSent : RequestPasswordResetControllerTestBase
    {
        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var result = await Controller.CodeSent("email@email.com");
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("CodeSent");
            result.As<ViewResult>().Model.Should().BeOfType<CodeSentViewModel>();
            result.As<ViewResult>().Model.As<CodeSentViewModel>().Email.Should().Be("email@email.com");
        }
    }
}