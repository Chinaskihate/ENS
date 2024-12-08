using ENS.Contracts.Exceptions;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.Resources.Errors;
using ENS.Common.Files;
using Microsoft.AspNetCore.Http;

namespace ENS.NotificationConfiguration.Services.Validation;
public class FileValidationService(FileValidationSettings settings) : IFileValidationService
{
    private readonly FileValidationSettings _settings = settings;

    public void Validate(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new InvalidFileException(Errors.EmptyFile, file?.FileName);
        }

        if (file.Length > _settings.MaxSizeInBytes)
        {
            throw new InvalidFileException(Errors.FileTooBig, file.FileName);
        }

        var extension = file.GetExtension();
        if (string.IsNullOrWhiteSpace(extension) || !_settings.AllowedExtensions.Contains(extension))
        {
            throw new InvalidFileException(Errors.UnsupportedExtension, file.FileName);
        }
    }
}