using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Web.Infrastructure;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class DevSetupController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILoginConfig _loginConfig;
        private readonly LoginContext _loginContext;
        private readonly ConfigurationDbContext _context;

        public DevSetupController(IHostingEnvironment environment, ILoginConfig loginConfig, LoginContext loginContext, ConfigurationDbContext context)
        {
            _environment = environment;
            _loginConfig = loginConfig;
            _loginContext = loginContext;
            _context = context;
        }
        
        [HttpPost("/DevSetup")]
        public IActionResult DevSetup()
        {
            if (!_environment.IsDevelopment())
            {
                return BadRequest("For development use only");
            }
            
            SeedData.EnsureSeedData(_loginConfig.SqlConnectionString, _loginContext, _context);
            
            return Ok();
        }
    }
}