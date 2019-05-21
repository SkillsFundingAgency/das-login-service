using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientByReturnUrl;
using SFA.DAS.LoginService.Web.Controllers.Login;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Login
{
    [TestFixture]
    public class When_POST_to_Login_with_invalid_credentials
    {
        private IMediator _mediator;
        private LoginController _controller;
        private readonly Guid _clientId = Guid.NewGuid();
        private string _returnUrl;

        [SetUp]
        public void SetUp()
        {
            _returnUrl = _clientId.ToString().Reverse().ToString(); // a fake returnUrl which is actually an encrypted string containing the client id

            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<ProcessLoginRequest>(), CancellationToken.None).Returns(new ProcessLoginResponse() {CredentialsValid = false, Message = "Invalid credentials"});
            _mediator.Send(Arg.Any<BuildLoginViewModelRequest>(), CancellationToken.None).Returns(new LoginViewModel());
            _mediator.Send(Arg.Any<GetClientByReturnUrlRequest>(), CancellationToken.None).Returns(new Client() {Id = _clientId});
            _controller = new LoginController(_mediator);
        }
        
        [Test]
        public async Task Then_result_should_be_ViewResult()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public async Task Then_ViewName_should_be_Login()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            ((ViewResult) result).ViewName.Should().Be("Login");
        }
        
        [Test]
        public async Task Then_ViewModel_should_be_LoginViewModel()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            ((ViewResult) result).Model.Should().BeOfType<LoginViewModel>();
        }
        
        [Test]
        public async Task Then_ModelState_IsValid_Should_Be_False()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            _controller.ModelState.IsValid.Should().BeFalse();
        }
        
        [Test]
        public async Task Then_ModelState_Should_Contain_Error_Message()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            _controller.ModelState["Username"].Errors[0].ErrorMessage.Should().Be("Invalid credentials");
        }

        [Test]
        public async Task Then_ReturnUrl_should_be_used_to_obtain_clientId()
        {
            await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            await _mediator.Received().Send(Arg.Is<GetClientByReturnUrlRequest>(p => p.ReturnUrl == _returnUrl));
        }
    }
}