using ENS.Contracts.NotificationConfiguration.Services;
using Microsoft.AspNetCore.Http;

namespace ENS.NotificationConfiguration.Services;
public class NotificationConfigurationService(IFileValidationService validationService) : INotificationConfigurationService
{
    private readonly IFileValidationService _validationService = validationService;

    public async Task ProcessFileAsync(IFormFile file)
    {
        _validationService.Validate(file);

        // do something with file...
    }
}
