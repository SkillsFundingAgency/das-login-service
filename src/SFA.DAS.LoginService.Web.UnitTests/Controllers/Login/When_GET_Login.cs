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