using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Web.Controllers.Login;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Login
{
    [TestFixture]
    public class When_POST_to_Login_with_empty_username_and_password
    {
        private LoginController _controller;

        [SetUp]
        public void SetUp()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<BuildLoginViewModelRequest>(), CancellationToken.None).Returns(new LoginViewModel()
            {
                Username = "",
                Password = "",
                RememberLogin = false,
                ReturnUrl = "https://returnurl",
                AllowRememberLogin = false,
                EnableLocalLogin = true
            });
            _controller = new LoginController(mediator);
            _controller.ViewData.ModelState.AddModelError("Password", "Password should be supplied");
            _controller.ViewData.ModelState.AddModelError("Username", "Username should be supplied");
        }
        
        [Test]
        public async Task Then_ViewResult_returned()
        {
            var result = await _controller.PostLogin(new LoginViewModel(){Password = "", Username = ""});
            result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public async Task Then_ViewName_should_be_Login()
        {  
            var result = await _controller.PostLogin(new LoginViewModel(){Password = "", Username = ""});   
            ((ViewResult) result).ViewName.Should().Be("Login");
        }
        
        [Test]
        public async Task Then_Model_should_be_LoginViewModel()
        {
            var result = await _controller.PostLogin(new LoginViewModel(){Password = "", Username = ""});
            ((ViewResult) result).Model.Should().BeOfType<LoginViewModel>();
        }

        [Test]
        public async Task Then_ModelState_should_not_be_valid()
        {
            var result = await _controller.PostLogin(new LoginViewModel(){Password = "", Username = ""});
            ((ViewResult) result).ViewData.ModelState.IsValid.Should().BeFalse();
        }
    }
}