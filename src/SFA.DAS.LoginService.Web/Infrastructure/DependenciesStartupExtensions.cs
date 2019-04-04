using System;
using System.Net;
using System.Net.Http;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.Configuration;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public static class DependenciesStartupExtensions
    {
        public static ILoginConfig WireUpDependencies(this IServiceCollection services, ILoginConfig loginConfig, IConfiguration configuration, IHostingEnvironment environment)
        {
            services.AddTransient<IConfigurationService, ConfigurationService>();

            loginConfig = new ConfigurationService()
                .GetLoginConfig(
                    configuration["EnvironmentName"],
                    configuration["ConfigurationStorageConnectionString"],
                    "1.0",
                    "SFA.DAS.LoginService", environment).Result;

            services.AddTransient(sp => loginConfig);

            if (environment.IsDevelopment())
            {
                services.AddTransient<IEmailService, EmailService.DevEmailService>();
            }
            else
            {
                services.AddHttpClient<IEmailService, EmailService.EmailService>();
            }

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<SignInManager<LoginUser>>();
            services.AddHttpClient<ICallbackService, CallbackService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            services.AddMediatR(typeof(CreateInvitationHandler).Assembly);

            return loginConfig;
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitterer = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
        }
    }
}