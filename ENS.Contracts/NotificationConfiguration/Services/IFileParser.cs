
using ENS.Contracts.NotificationConfiguration.Models;

namespace ENS.Contracts.NotificationConfiguration.Services;
public interface IFileParser
{
    public List<NotificationConfigurationDto> Parse(Stream fileStream);
}
