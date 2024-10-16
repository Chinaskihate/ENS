using Microsoft.AspNetCore.Mvc;

namespace ENS.NotificationConfigurationService.Controllers;
[ApiController]
[Route("[controller]")]
public class NotificationConfigurationController : ControllerBase
{
    private readonly ILogger<NotificationConfigurationController> _logger;

    public NotificationConfigurationController(ILogger<NotificationConfigurationController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAsync(string userId)
    {
        return Ok(userId);
    }
}
