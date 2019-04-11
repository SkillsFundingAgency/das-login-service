using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.CreateAccountController;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreateAccount
{
    [TestFixture]
    public class CreateAccountTestBase
    {
        protected IMediator Mediator;
        protected CreateAccountController Controller;
        
        [SetUp]
        public void SetUp()
        {
            Mediator = Substitute.For<IMediator>();
            Controller = new CreateAccountController(Mediator);
        }
    }
}