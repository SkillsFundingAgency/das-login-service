using System.Security.Claims;

namespace SFA.DAS.LoginService.Application.UnitTests.Helpers
{
    public class TestIdentity : ClaimsIdentity
    {
        public override string Name => "User";

        public TestIdentity(params Claim[] claims) : base(claims)
        {
                
        }
    }
}