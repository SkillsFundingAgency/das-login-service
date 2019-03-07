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
        private ResetPasswordController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new ResetPasswordController();
        }
        
        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var result = await _controller.Get("ClientID");
            result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public async Task Then_ViewResult_is_returned_with_correct_view()
        {
            var result = await _controller.Get("ClientID");
            ((ViewResult) result).ViewName.Should().Be("ResetPassword");
        }
        
        [Test]
        public async Task Then_ViewResult_is_returned_with_correct_viewmodel()
        {
            var result = await _controller.Get("ClientID");
            ((ViewResult) result).Model.Should().BeOfType<ResetPasswordViewModel>();
            ((ResetPasswordViewModel))
        }
    }
}