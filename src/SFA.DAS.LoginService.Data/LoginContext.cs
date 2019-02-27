using System;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Data
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> dbContextOptions) : base(dbContextOptions){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invitation>()
                .Property(i => i.CallbackUri)
                .HasConversion(
                    v => v.ToString(), 
                    v => new Uri(v));

            modelBuilder.Entity<Invitation>()
                .Property(i => i.UserRedirectUri)
                .HasConversion(
                    v => v.ToString(),
                    v => new Uri(v));
        }

        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}