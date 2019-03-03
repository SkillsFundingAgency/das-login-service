using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Web.Controllers.Login;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Login
{
    [TestFixture]
    public class When_POST_to_Login_with_invalid_credentials
    {
        private IMediator _mediator;
        private LoginController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<ProcessLoginRequest>(), CancellationToken.None).Returns(new ProcessLoginResponse() {CredentialsValid = false});
            _controller = new LoginController(_mediator);
        }
        
        [Test]
        public async Task Then_result_should_be_ViewResult()
        {
            var result = await _controller.PostLogin(new LoginViewModel());
            result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public async Task Then_ViewName_should_be_Login()
        {
            var result = await _controller.PostLogin(new LoginViewModel());
            ((ViewResult) result).ViewName.Should().Be("Login");
        }
        
        [Test]
        public async Task Then_ViewModel_should_be_LoginViewModel()
        {
            var result = await _controller.PostLogin(new LoginViewModel());
            ((ViewResult) result).Model.Should().BeOfType<LoginViewModel>();
        }
        
        [Test]
        public async Task Then_ModelState_IsValid_Should_Be_False()
        {
            await _controller.PostLogin(new LoginViewModel());
            _controller.ModelState.IsValid.Should().BeFalse();
        }
        
        [Test]
        public async Task Then_ModelState_Should_Contain_Error_Message()
        {
            await _controller.PostLogin(new LoginViewModel());
            _controller.ModelState[""].Errors[0].ErrorMessage.Should().Be("Invalid credentials");
        }
        
    }
}