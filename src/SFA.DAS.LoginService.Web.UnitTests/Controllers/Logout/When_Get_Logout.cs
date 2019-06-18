using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLogoutViewModel;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientByLogoutId;
using SFA.DAS.LoginService.Web.Controllers.Logout;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Logout
{
    [TestFixture]
    public class When_Get_Logout
    {
        private IMediator _mediator;
        private LogoutController _controller;
        private readonly Guid _clientId = Guid.NewGuid();
        private string _logoutId;

        [SetUp]
        public void SetUp()
        {
            _logoutId = Convert.ToBase64String(_clientId.ToByteArray()); // a fake returnUrl which is actually an encrypted string containing the client id

            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<LogoutRequest>(), CancellationToken.None).Returns(new LogoutResponse());
            _mediator.Send(Arg.Any<GetClientByLogoutIdRequest>(), CancellationToken.None).Returns(new Client()
            {
                Id = _clientId
            });
            _controller = new LogoutController(_mediator);
        }
        
        [Test]
        public async Task Then_mediator_is_called_to_build_logout_viewmodel()
        {
            await _controller.Get(_logoutId);
            await _mediator.Send(Arg.Is<LogoutRequest>(r => r.LogoutId == _logoutId));
        }
        
        [Test]
        public async Task Then_ViewResult_Is_returned()
        {
            var result = await _controller.Get(_logoutId);
            result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public async Task Then_ViewResult_Is_returned_with_correct_view()
        {
            var result = await _controller.Get(_logoutId);

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("Loggedout");
        }
        
        [Test]
        public async Task Then_ViewResult_Is_returned_with_LogoutViewModel()
        {
            var result = await _controller.Get(_logoutId);

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).Model.Should().BeOfType<LogoutResponse>();
        }

        [Test]
        public async Task Then_LogoutId_should_be_used_to_obtain_clientId()
        {
            await _controller.Get(_logoutId);
            await _mediator.Received().Send(Arg.Is<GetClientByLogoutIdRequest>(p => p.LogoutId == _logoutId));
        }
    }   
}