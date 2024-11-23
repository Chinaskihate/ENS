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
    public async Task<IActionResult> GetAsync(string userId)
    {
        return Ok(userId);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        // Only allow .csv or .xlsx files 
        if (extension != ".csv" && extension != ".xlsx")
        {
            return BadRequest("Only .csv and .xlsx files are allowed.");
        }

        // do something with file...

        return Ok(new { Message = "File uploaded successfully" });
    }
}
