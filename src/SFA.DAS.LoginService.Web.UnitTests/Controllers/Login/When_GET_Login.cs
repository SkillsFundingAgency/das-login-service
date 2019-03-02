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
    
    [TestFixture]
    public class When_GET_Login
    {
        private IMediator _mediator;
        private LoginController _controller;
        private string _returnUrl;

        [SetUp]
        public void SetUp()
        {
            _returnUrl = "https://localhost/redirecturl";
            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<BuildLoginViewModelRequest>(), CancellationToken.None).Returns(new LoginViewModel()
            {
                Username = "",
                Password = "",
                RememberLogin = false,
                ReturnUrl = _returnUrl,
                AllowRememberLogin = false,
                EnableLocalLogin = true
            });
            _controller = new LoginController(_mediator);
        }
        
        [Test]
        public async Task Then_mediator_is_called_to_build_login_viewmodel()
        {
            await _controller.GetLogin(_returnUrl);
            await _mediator.Received().Send(Arg.Is<BuildLoginViewModelRequest>(r => r.returnUrl == _returnUrl), CancellationToken.None);
        }

        [Test]
        public async Task Then_ViewResult_is_returned_with_correct_model()
        {
            var result = await _controller.GetLogin(_returnUrl);
            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("Login");
            ((ViewResult) result).Model.Should().BeOfType<LoginViewModel>();
        }
    }
}