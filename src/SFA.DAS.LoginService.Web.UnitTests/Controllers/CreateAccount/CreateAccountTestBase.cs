using System;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class CreatePasswordTestsBase
    {
        protected IMediator Mediator;
        protected CreatePasswordController Controller;
        
        [SetUp]
        public void SetUp()
        {
            Mediator = Substitute.For<IMediator>();
            Controller = new CreatePasswordController(Mediator);
            SystemTime.UtcNow = () => new DateTime(2019,1,1,1,1,1);
        }
    }
}