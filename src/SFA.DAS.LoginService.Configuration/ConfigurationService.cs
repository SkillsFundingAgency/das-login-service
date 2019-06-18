using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SFA.DAS.LoginService.Data.JsonObjects;
using SFA.DAS.LoginService.Types.GetClientById;

namespace SFA.DAS.LoginService.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IMediator _mediator;
       
        public ConfigurationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<LoginConfig> GetNewLoginConfig(string environmentName, string storageConnectionString,
            string version,
            string serviceName, IHostingEnvironment environment)
        {
            return (await GetLoginConfig(environmentName, storageConnectionString, version, serviceName, environment)) as LoginConfig;
        }

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
                        "There is a mismatch between LoginConfig:ILoginConfig and the JSON returned from storage.");
                }

                throw;
            }

            return loginConfig;
        }

        public async Task<IClientConfig> GetClientConfig(Guid? clientId)
        {
            ClientConfig config = null;

            if(clientId != null)
            {
                var client = await _mediator.Send(new GetClientByIdRequest {ClientId = clientId.Value});
                if (client != null)
                {
                    config = new ClientConfig
                    {
                        ServiceDetails = client.ServiceDetails,
                        AllowInvitationSignUp = client.AllowInvitationSignUp,
                        AllowLocalSignUp = client.AllowLocalSignUp
                    };
                }
            }

            return config ?? new ClientConfig
            {
                ServiceDetails = new ServiceDetails()
            };
        }

        public class DeveloperEnvironmentException : Exception
        {
            public DeveloperEnvironmentException(string message) : base(message)
            {
            }
        }
    }
}
