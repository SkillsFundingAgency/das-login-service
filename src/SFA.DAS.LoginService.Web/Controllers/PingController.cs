using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class PingController : Controller
    {
        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("/Ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping -> Pong");
            return Ok("Pong");
        }
    }
}