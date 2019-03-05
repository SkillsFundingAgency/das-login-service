using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLogoutViewModel;
using SFA.DAS.LoginService.Web.Controllers.Logout;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Logout
{
    [TestFixture]
    public class When_Get_Logout
    {
        private IMediator _mediator;
        private LogoutController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<LogoutRequest>(), CancellationToken.None).Returns(new LogoutResponse());
            _controller = new LogoutController(_mediator);
        }
        
        [Test]
        public async Task Then_mediator_is_called_to_build_logout_viewmodel()
        {
            await _controller.Get("logoutid");
            await _mediator.Send(Arg.Is<LogoutRequest>(r => r.LogoutId == "logoutid"));
        }
        
        [Test]
        public async Task Then_ViewResult_Is_returned()
        {
            var result = await _controller.Get("logoutid");
            result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public async Task Then_ViewResult_Is_returned_with_correct_view()
        {
            var result = await _controller.Get("logoutid");

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("Loggedout");
        }
        
        [Test]
        public async Task Then_ViewResult_Is_returned_with_LogoutViewModel()
        {
            var result = await _controller.Get("logoutid");

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).Model.Should().BeOfType<LogoutResponse>();
        }
    }   
}