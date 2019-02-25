using System;
using System.Linq;
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
    public class When_ConfirmCode_called_with_valid_id_and_code
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
        public void Then_ConfirmCodeResponse_is_returned()
        {
            var result = _handler.Handle(new ConfirmCodeViewModel(_invitationId, "code"), CancellationToken.None).Result;
            
            result.Should().BeOfType<ConfirmCodeResponse>();
        }

        [Test]
        public void Then_ConfirmCodeResponse_IsValid_is_true()
        {
            var result = _handler.Handle(new ConfirmCodeViewModel(_invitationId, "code"), CancellationToken.None).Result;

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Then_Invitation_record_is_updated_with_CodeConfirmed_true()
        {
            _handler.Handle(new ConfirmCodeViewModel(_invitationId, "code"), CancellationToken.None).Wait();
            var invitation = _loginContext.Invitations.First(i => i.Id == _invitationId);
            
            invitation.CodeConfirmed.Should().BeTrue();
        }
    }
}