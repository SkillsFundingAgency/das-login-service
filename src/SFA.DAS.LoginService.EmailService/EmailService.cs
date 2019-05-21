using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Client.Configuration;
using SFA.DAS.Notifications.Api.Types;

namespace SFA.DAS.LoginService.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly NotificationsApi _notificationApi;

        public EmailService(HttpClient httpClient, ILogger<EmailService> logger, ILoginConfig config)
        {
            _logger = logger;

            var notificationsApiClientConfiguration = new NotificationsApiClientConfiguration()
            {
                ApiBaseUrl = config.NotificationsApiClientConfiguration.ApiBaseUrl,
                #pragma warning disable 618
                ClientToken = config.NotificationsApiClientConfiguration.ClientToken,
                #pragma warning restore 618
                ClientId = config.NotificationsApiClientConfiguration.ClientId,
                ClientSecret = config.NotificationsApiClientConfiguration.ClientSecret,
                IdentifierUri = config.NotificationsApiClientConfiguration.IdentifierUri,
                Tenant = config.NotificationsApiClientConfiguration.Tenant,
            };

            if (string.IsNullOrWhiteSpace(config.NotificationsApiClientConfiguration.ClientId))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.NotificationsApiClientConfiguration.ClientToken);
            }
            else
            {
                var azureAdToken = new AzureADBearerTokenGenerator(notificationsApiClientConfiguration).Generate().Result;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureAdToken);
            }
                
            
            _notificationApi = new NotificationsApi(httpClient, notificationsApiClientConfiguration);
        }
        
        public async Task SendInvitationEmail(InvitationEmailViewModel viewModel)
        {
            await SendEmail(viewModel);
        }

        public async Task SendResetPassword(ResetPasswordEmailViewModel viewModel)
        {
            await SendEmail(viewModel);
        }

        public async Task SendResetNoAccountPassword(PasswordResetNoAccountEmailViewModel viewModel)
        {           
            await SendEmail(viewModel);
        }

        public async Task SendPasswordReset(PasswordResetEmailViewModel viewModel)
        {
            await SendEmail(viewModel);
        }

        public async Task SendUserExistsEmail(UserExistsEmailViewModel viewModel)
        {
            await SendEmail(viewModel);
        }

        private async Task SendEmail(EmailViewModel viewModel)
        {
            var tokens = GetTokens(viewModel);

            await _notificationApi.SendEmail(new Email()
            {
                RecipientsAddress = viewModel.EmailAddress,
                TemplateId = viewModel.TemplateId.ToString(),
                Tokens = tokens,
                SystemId = "ApplyService",
                ReplyToAddress = "digital.apprenticeship.service@notifications.service.gov.uk",
                Subject = viewModel.Subject
            });
        }
        
        private Dictionary<string, string> GetTokens(EmailViewModel vm)
        {
            return vm.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(vm).ToString());
        }
    }
}