using ENS.Common.Files;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.Contracts.NotificationConfiguration.Services.Parsing;
using ENS.Serialization;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ENS.NotificationConfiguration.Services;
public class NotificationConfigurationService(
    IFileValidationService validationService,
    IFileParserFactory parserFactory) : INotificationConfigurationService
{
    private readonly IFileValidationService _validationService = validationService;
    private readonly IFileParserFactory _parserFactory = parserFactory;

    public async Task ProcessFileAsync(IFormFile file)
    {
        _validationService.Validate(file);

        await ProcessFileInternalAsync(file);
    }

    private async Task ProcessFileInternalAsync(IFormFile file)
    {
        var parser = _parserFactory.GetParser(file.GetExtension());
        using var fileStream = file.OpenReadStream();
        var records = parser.Parse(fileStream);
        foreach (var record in records)
        {
            // Process each record as needed
            Log.Information(record.ToJson());
        }
    }
}