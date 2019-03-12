using System;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString, LoginContext loginContext, ConfigurationDbContext context)
        {
//            var services = new ServiceCollection();
//            services.AddOperationalDbContext(options =>
//            {
//                options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
//            });
//            services.AddConfigurationDbContext(options =>
//            {
//                options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
//            });
//
//            
            
                EnsureSeedData(context, loginContext);
            
        }
        
        private static void EnsureSeedData(IConfigurationDbContext context, LoginContext loginContext)
        {
            Console.WriteLine("Seeding database...");

            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Config.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApis().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }

            if (!loginContext.Clients.Any())
            {
                Console.WriteLine("LoginService.Clients being populated");
                loginContext.Clients.Add(new Client()
                {
                    Id = Guid.Parse("2350df68-e325-4ccc-9027-e1051e48d4a7"),
                    ServiceDetails = new ServiceDetails {ServiceName = "Apply Service", SupportUrl = "https://acme.gov.uk/support", PostPasswordResetReturnUrl = "https://localhost:6016"},
                    IdentityServerClientId = "apply",
                    AllowInvitationSignUp = true,
                    AllowLocalSignUp = false
                });
                loginContext.Clients.Add(new Client()
                {
                    Id = Guid.Parse("08372e20-becd-415c-9925-4d33ddf67faf"),
                    ServiceDetails = new ServiceDetails {ServiceName = "Assessor Service", SupportUrl = "https://localhost:5015/support", PostPasswordResetReturnUrl = "https://localhost:5015"},
                    IdentityServerClientId = "assessor",
                    AllowInvitationSignUp = true,
                    AllowLocalSignUp = false
                });
                loginContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("LoginService.Clients already populated");
            }
            
            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}