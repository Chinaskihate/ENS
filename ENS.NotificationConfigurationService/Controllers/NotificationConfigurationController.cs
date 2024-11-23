using ENS.Contracts;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.Resources.Messages;
using Microsoft.AspNetCore.Mvc;

namespace ENS.NotificationConfigurationService.Controllers;
[ApiController]
[Route("[controller]")]
public class NotificationConfigurationController(INotificationConfigurationService configurationService) : ControllerBase
{
    private readonly INotificationConfigurationService _configurationService = configurationService;

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file)
    {
        await _configurationService.ProcessFileAsync(file);

        return Ok(new ResponseDto
        {
            Message = Messages.FileUploaded
        });
    }
}
