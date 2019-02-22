using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Data
{
    public class LoginUserContext : IdentityDbContext<LoginUser>
    {
        public LoginUserContext(DbContextOptions<LoginUserContext> options)
            : base(options)
        {
        }
    }
}