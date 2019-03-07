using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ResetPassword
{
    [TestFixture]
    public class When_GET_called_on_reset_password
    {
        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var controller = new ResetPasswordController();
            var result = await controller.Get("ClientID");
            result.Should().BeOfType<ViewResult>();
        }
    }
}