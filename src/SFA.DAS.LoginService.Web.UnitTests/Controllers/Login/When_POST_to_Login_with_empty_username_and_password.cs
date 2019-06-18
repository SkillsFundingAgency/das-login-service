using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.EntityFramework.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Types.GetClientByReturnUrl;
using SFA.DAS.LoginService.Web.Controllers.Login;
using Client = SFA.DAS.LoginService.Data.Entities.Client;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Login
{
    [TestFixture]
    public class When_POST_to_Login_with_empty_username_and_password
    {
        private LoginController _controller;
        private IMediator _mediator;
        private readonly Guid _clientId = Guid.NewGuid();
        private string _returnUrl;

        [SetUp]
        public void SetUp()
        {
            _returnUrl = Convert.ToBase64String(_clientId.ToByteArray()); // a fake returnUrl which is actually an encrypted string containing the client id

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
            _mediator.Send(Arg.Any<GetClientByReturnUrlRequest>(), CancellationToken.None).Returns(new Client()
            {
                Id = _clientId
            });
            _controller = new LoginController(_mediator);
            _controller.ViewData.ModelState.AddModelError("Password", "Password should be supplied");
            _controller.ViewData.ModelState.AddModelError("Username", "Username should be supplied");
        }

        [Test]
        public async Task Then_ViewResult_returned()
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
        public async Task Then_Model_should_be_LoginViewModel()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            ((ViewResult) result).Model.Should().BeOfType<LoginViewModel>();
        }

        [Test]
        public async Task Then_ModelState_should_not_be_valid()
        {
            var result = await _controller.PostLogin(new LoginViewModel() { Password = "", Username = "", ReturnUrl = _returnUrl });
            ((ViewResult) result).ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Test]
        public async Task Then_ReturnUrl_should_be_used_to_obtain_clientId()
        {
            await _controller.PostLogin(new LoginViewModel() {Password = "", Username = "", ReturnUrl = _returnUrl});
            await _mediator.Received().Send(Arg.Is<GetClientByReturnUrlRequest>(p => p.ReturnUrl == _returnUrl));
        }
    }
}