using Microsoft.AspNetCore.Http;

namespace ENS.Contracts.NotificationConfiguration.Services;
public interface INotificationConfigurationService
{
    Task ProcessFileAsync(IFormFile file);
}
