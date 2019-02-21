using System;
using Microsoft.EntityFrameworkCore;

namespace SFA.DAS.LoginService.Data
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> dbContextOptions) : base(dbContextOptions){}
        public DbSet<Invitation> Invitations { get; set; }
    }

    public class Invitation
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string SourceId { get; set; }
        public string Code { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}