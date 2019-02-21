using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.UnitTests.Helpers;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.HashingServiceTests
{
    [TestFixture]
    public class When_hash_is_requested
    {
        [Test]
        public void Then_a_hash_is_returned()
        {
            var hashingService = new HashingService();
            var hash = hashingService.GetHash("PlainText");
            hash.Should().NotBeEmpty();
        }

        [Test]
        public void Then_the_correct_hash_is_returned()
        {
            var expectedHash = "PlainText".GenerateHash();

            var hashingService = new HashingService();
            var hash = hashingService.GetHash("PlainText");
            hash.Should().Be(expectedHash);
        }
    }
}