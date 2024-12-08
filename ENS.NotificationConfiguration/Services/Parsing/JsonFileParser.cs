using ENS.Contracts.NotificationConfiguration.Models;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.NotificationConfiguration.Attributes;
using ENS.Serialization;

namespace ENS.NotificationConfiguration.Services.Parsing;

[FileParser("json")]
public class JsonFileParser : IFileParser
{
    public List<NotificationConfigurationDto> Parse(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var jsonContent = reader.ReadToEnd();
        return jsonContent.ToType<List<NotificationConfigurationDto>>();
    }
}