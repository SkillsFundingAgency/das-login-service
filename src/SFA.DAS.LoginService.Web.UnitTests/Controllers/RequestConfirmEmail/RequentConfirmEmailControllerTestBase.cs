using System;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmEmail
{
    [TestFixture]
    public class RequentConfirmEmailControllerTestBase
    {
        protected RequestConfirmEmailController Controller;
        protected Guid ClientId;
        protected IMediator Mediator;

        [SetUp]
        public void Arrange()
        {
            Mediator = Substitute.For<IMediator>();
            Controller = new RequestConfirmEmailController(Mediator);
            ClientId = Guid.NewGuid(); // what is this used for??
        }
    }
}