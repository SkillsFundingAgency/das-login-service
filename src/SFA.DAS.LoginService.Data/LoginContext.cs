using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Data
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> dbContextOptions) : base(dbContextOptions){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("LoginService");
            
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

            modelBuilder.Entity<Client>()
                .Property(c => c.ServiceDetails)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<ServiceDetails>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }

        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<InvalidPassword> InvalidPasswords { get; set; }
    }
}