using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace SFA.DAS.LoginService.Application.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public async Task<ILoginConfig> GetLoginConfig(string environmentName, string storageConnectionString, string version,
            string serviceName, IHostingEnvironment environment)
        {
            if (environmentName == null || storageConnectionString == null)
            {
                if (environment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        @"Cannot find settings 'EnvironmentName' and 'ConfigurationStorageConnectionString' in appsettings.json. Please ensure your appsettings.json file is at least set up like `{
                    ""Logging"": {
                        ""IncludeScopes"": false,
                        ""LogLevel"": {
                            ""Default"": ""Debug"",
                            ""System"": ""Information"",
                            ""Microsoft"": ""Information""
                        }
                    },
                    ""ConfigurationStorageConnectionString"": ""UseDevelopmentStorage=true;"",
                    ""ConnectionStrings"": {
                        ""Redis"": """"
                    },
                    ""EnvironmentName"": ""LOCAL""
                }`");
                }

                throw new Exception(
                    "Cannot find settings 'EnvironmentName' and 'ConfigurationStorageConnectionString'");
            }

            var conn = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = conn.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Configuration");

            var operation = TableOperation.Retrieve(environmentName, $"{serviceName}_{version}");
            TableResult result;
            try
            {
                result = await table.ExecuteAsync(operation);
            }
            catch (Exception e)
            {
                if (environment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        "Could not connect to Storage to retrieve settings.  Please ensure you have a Azure Storage server (azure or local emulator) configured with a `Configuration` table and the correct row.  See README.MD for details.");
                }

                throw new Exception("Could not connect to Storage to retrieve settings.", e);
            }

            var dynResult = result.Result as DynamicTableEntity;
            if (result.HttpStatusCode == StatusCodes.Status404NotFound)
            {
                if (environment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        "Cannot open Configuration table. Please ensure you have a Azure Storage server (azure or local emulator) configured with a `Configuration` table and the correct row.  See README.MD for details.");
                }

                throw new Exception("Cannot open Configuration table.");
            }

            var data = dynResult.Properties["Data"].StringValue;

            LoginConfig loginConfig;
            try
            {
                loginConfig = JsonConvert.DeserializeObject<LoginConfig>(data);
            }
            catch (Exception)
            {
                if (environment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        "There is a mismatch between ApplyConfig:IApplyConfig and the JSON returned from storage.");
                }

                throw;
            }

            return loginConfig;
        }
        
        public class DeveloperEnvironmentException : Exception
        {
            public DeveloperEnvironmentException(string message) : base(message)
            {
            
            }
        }
    }
}