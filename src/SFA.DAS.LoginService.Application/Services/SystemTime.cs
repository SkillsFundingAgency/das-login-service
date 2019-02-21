using System;

namespace SFA.DAS.LoginService.Application.Services
{
    public class SystemTime
    {
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}