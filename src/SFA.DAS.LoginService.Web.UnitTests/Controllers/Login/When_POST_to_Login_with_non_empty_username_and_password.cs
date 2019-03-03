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
    public class When_POST_to_Login_with_valid_credentials
    {
        private IMediator _mediator;
        private LoginController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<ProcessLoginRequest>(), CancellationToken.None).Returns(new ProcessLoginResponse() {CredentialsValid = true});
            _controller = new LoginController(_mediator);
        }
        
        [Test]
        public async Task Then_mediator_is_called_with_correct_arguments()
        {
            await _controller.PostLogin(new LoginViewModel()
            {
                Username = "myuser@name.com", 
                Password = "Pa55word!", 
                RememberLogin = false,
                ReturnUrl = "https://returnurl"
            });

            await _mediator.Received().Send(Arg.Is<ProcessLoginRequest>(r =>
                r.Username == "myuser@name.com"
                && r.Password == "Pa55word!"
                && r.RememberLogin == false
                && r.ReturnUrl == "https://returnurl"), CancellationToken.None);
        }

        [Test]
        public async Task Then_result_should_be_RedirectResult()
        {
            var result = await _controller.PostLogin(new LoginViewModel(){ReturnUrl = "https://returnurl"});
            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult) result).Url.Should().Be("https://returnurl");
        }
    }
}