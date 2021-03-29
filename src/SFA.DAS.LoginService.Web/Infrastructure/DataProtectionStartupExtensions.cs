using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.LoginService.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public static class DataProtectionStartupExtensions
    {
        public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environment, IServiceProvider serviceProvider)
        {
            if (!environment.IsDevelopment())
            {
                ILoginConfig loginConfig = new ConfigurationService(serviceProvider.GetService<IMediator>())
                .GetLoginConfig(
                    configuration["EnvironmentName"],
                    configuration["ConfigurationStorageConnectionString"],
                    "1.0",
                    "SFA.DAS.LoginService", environment).Result;

                if (loginConfig != null)
                {
                    var defaultSessionRedisConnectionString = loginConfig.DefaultSessionRedisConnectionString;
                    var dataProtectionKeysDatabase = loginConfig.DataProtectionKeysDatabase;

                    var redis = ConnectionMultiplexer
                        .Connect($"{defaultSessionRedisConnectionString},{dataProtectionKeysDatabase}");

                    services.AddDataProtection()
                        .SetApplicationName("das-login-service")
                        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
                }
            }
            return services;
        }
    }
}
