using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ResetPassword
{
    public class When_GET_called_on_reset_password : ResetPasswordControllerTestBase
    {
        private IActionResult _result;

        [SetUp]
        public async Task Act()
        {       
            _result = await Controller.Get(ClientId.ToString());
        }       
        
        [Test]
        public void Then_ViewResult_is_returned()
        {
            _result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public void Then_ViewResult_is_returned_with_correct_view()
        {
            ((ViewResult) _result).ViewName.Should().Be("ResetPassword");
        }
        
        [Test]
        public void Then_ViewResult_is_returned_with_correct_viewmodel()
        {
            ((ViewResult) _result).Model.Should().BeOfType<ResetPasswordViewModel>();
            ((ResetPasswordViewModel) ((ViewResult) _result).Model).ClientId.Should().Be(ClientId);
        }
    }
}