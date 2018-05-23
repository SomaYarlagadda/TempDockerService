using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TempDockerService.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public string Index()
        {
            _logger.LogWarning("Inside the Health check");
            _logger.LogError("Done processing");
            return "I am up";
        }
    }
}
