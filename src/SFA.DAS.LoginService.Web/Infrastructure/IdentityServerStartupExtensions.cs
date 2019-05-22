using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services.Configuration;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public static class IdentityServerStartupExtensions
    {
        public static void AddIdentityServer(this IServiceCollection services, ILoginConfig loginConfig, IHostingEnvironment environment, ILogger logger)
        {
            services.AddScoped<IPasswordHasher<LoginUser>, LoginServicePasswordHasher<LoginUser>>();

            services.AddIdentity<LoginUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Lockout.MaxFailedAccessAttempts = loginConfig.MaxFailedAccessAttempts;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(14);
                        options.SignIn.RequireConfirmedEmail = true;
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
                    options.ConfigureDbContext = builder => builder.UseSqlServer(loginConfig.SqlConnectionString);
                    options.DefaultSchema = "IdentityServer";
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(loginConfig.SqlConnectionString);
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