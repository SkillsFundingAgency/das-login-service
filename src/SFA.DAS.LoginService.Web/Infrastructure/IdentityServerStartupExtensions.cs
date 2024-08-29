using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public static class IdentityServerStartupExtensions
    {
        public static void AddIdentityServer(this IServiceCollection services, ILoginConfig loginConfig, IHostingEnvironment environment, ILogger logger)
        {
            services.AddIdentity<LoginUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Lockout.MaxFailedAccessAttempts = loginConfig.MaxFailedAccessAttempts;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(14);
                    })
                .AddPasswordValidator<CustomPasswordValidator<LoginUser>>()
                .AddEntityFrameworkStores<LoginUserContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".Login.Identity.Application";
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            var isBuilder = services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction.ErrorUrl = "/Error";
                })
                .AddConfigurationStore(options =>
                {
                    var connection = new System.Data.SqlClient.SqlConnection(loginConfig.SqlConnectionString);

                    if (!environment.IsDevelopment())
                    {
                        var generateTokenTask = SqlTokenGenerator.GenerateTokenAsync();
                        connection.AccessToken = generateTokenTask.GetAwaiter().GetResult();
                    }

                    options.ConfigureDbContext = builder => builder.UseSqlServer(connection);
                    options.DefaultSchema = "IdentityServer";
                })
                .AddOperationalStore(options =>
                {
                    var connection = new System.Data.SqlClient.SqlConnection(loginConfig.SqlConnectionString);

                    if (!environment.IsDevelopment())
                    {
                        var generateTokenTask = SqlTokenGenerator.GenerateTokenAsync();
                        connection.AccessToken = generateTokenTask.GetAwaiter().GetResult();
                    }

                    options.ConfigureDbContext = builder => builder.UseSqlServer(connection);
                    options.DefaultSchema = "IdentityServer";
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<LoginUser>();

            if (environment.IsDevelopment())
            {
                isBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                isBuilder.AddCertificateFromStore(loginConfig.CertificateThumbprint, logger);
            }
        }
    }
}