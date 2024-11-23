using Microsoft.AspNetCore.Http;

namespace ENS.Contracts.NotificationConfiguration.Services;
public interface IFileValidationService
{
    void Validate(IFormFile file);
}
