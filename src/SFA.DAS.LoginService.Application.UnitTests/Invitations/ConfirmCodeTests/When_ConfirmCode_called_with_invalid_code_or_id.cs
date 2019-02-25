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
    public class When_ConfirmCode_called_with_invalid_code_or_id
    {
        private LoginContext _loginContext;
        private Guid _invitationId;
        private ConfirmCodeHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "ConfirmCodeHandler_tests")
                .Options;

            _loginContext = new LoginContext(dbContextOptions);

            _invitationId = Guid.NewGuid();
            _loginContext.Invitations.Add(new Invitation()
            {
                Id = _invitationId,
                Code = "code".GenerateHash()
            });
            _loginContext.SaveChanges();
            
            _handler = new ConfirmCodeHandler(_loginContext, new HashingService());
        }
        
        [Test]
        public void Then_Invalid_Code_ConfirmCodeResponse_IsValid_is_false()
        {
            var result = _handler.Handle(new ConfirmCodeRequest(_invitationId, "ABADCODE"), CancellationToken.None).Result;

            result.IsValid.Should().BeFalse();
        }
        
        [Test]
        public void Then_Invalid_Id_ConfirmCodeResponse_IsValid_is_false()
        {
            var result = _handler.Handle(new ConfirmCodeRequest(Guid.NewGuid(), "code"), CancellationToken.None).Result;

            result.IsValid.Should().BeFalse();
        }
    }
}