using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IConfigurationService
    {
        Task<ILoginConfig> GetLoginConfig(string environmentName, string storageConnectionString, string version,
            string serviceName, IHostingEnvironment environment);
    }
}