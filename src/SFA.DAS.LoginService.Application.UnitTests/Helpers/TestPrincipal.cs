using System.Security.Claims;
using SFA.DAS.LoginService.Application.UnitTests.Logout;

namespace SFA.DAS.LoginService.Application.UnitTests.Helpers
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
        {
        }
    }
}