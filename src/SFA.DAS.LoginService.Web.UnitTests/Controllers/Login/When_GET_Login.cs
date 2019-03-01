using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.Login;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Login
{
    [TestFixture]
    public class When_GET_Login
    {
        [Test]
        public void Then_mediator_is_called_to_build_login_viewmodel()
        {
            var mediator = Substitute.For<IMediator>();
            var controller = new LoginController(mediator);
            mediator.Received().Send(new BuildLoginViewModelRequest());
        }
    }
}