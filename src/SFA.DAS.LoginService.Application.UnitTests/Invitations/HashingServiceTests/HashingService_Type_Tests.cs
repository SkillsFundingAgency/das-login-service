using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.HashingServiceTests
{
    [TestFixture]
    public class HashingService_Type_Tests
    {
        [Test]
        public void HashingService_implements_IHashingService()
        {
            typeof(HashingService).Should().Implement<IHashingService>();
        }
    }
}