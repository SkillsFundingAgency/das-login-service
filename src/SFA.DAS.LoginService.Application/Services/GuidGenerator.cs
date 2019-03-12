using System;

namespace SFA.DAS.LoginService.Application.Services
{
    public class GuidGenerator
    {
        public static Func<Guid> NewGuid = Guid.NewGuid;
    }
}