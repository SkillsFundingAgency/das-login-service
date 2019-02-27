using System;
using System.Threading;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.UnitTests.Helpers;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.ConfirmCodeTests
{
    [TestFixture]
    public class When_ConfirmCode_called_for_completed_invitation
    {
        [Test]
        public void Then_result_should_be_false()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "ConfirmCodeHandler_tests")
                .Options;

            var loginContext = new LoginContext(dbContextOptions);

            var invitationId = Guid.NewGuid();
            loginContext.Invitations.Add(new Invitation()
            {
                Id = invitationId,
                Code = "code".GenerateHash(),
                IsUserCreated = true
            });
            loginContext.SaveChanges();
            
            var handler = new ConfirmCodeHandler(loginContext, new HashingService());

            var result = handler.Handle(new ConfirmCodeRequest(invitationId, "code"), CancellationToken.None).Result;

            result.IsValid.Should().BeFalse();
        }
    }
}