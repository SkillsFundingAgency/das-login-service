using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web.Controllers.MigrationsApi
{
    public class MigrationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<LoginUser> _userManager;
        private readonly ILogger<MigrationsController> _logger;
        private readonly LoginContext _loginContext;

        public MigrationsController(IMediator mediator, UserManager<LoginUser> userManager, ILogger<MigrationsController> logger, LoginContext loginContext)
        {
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
            _loginContext = loginContext;
        }
        
        [HttpPost("/Migrate")]
        public async Task<IActionResult> Migrate([FromBody] MigrateUser user)
        {
            var clientId = _loginContext.Clients.Single(c => c.IdentityServerClientId == user.ClientId).Id;
            
            var newUser = new LoginUser()
            {
                Email = user.Email, 
                UserName = user.Email, 
                FamilyName = user.FamilyName, 
                GivenName = user.GivenName
            };
            
            var result = await _userManager.CreateAsync(newUser, Guid.NewGuid().ToString());

            if (!result.Succeeded)
            {
                _logger.LogError($"User {user.Email} could not be created: {result}");
            }

            return Ok(new {newUserId = newUser.Id});
        }
    }

    public class MigrateUser
    {
        public string ClientId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
    }
}