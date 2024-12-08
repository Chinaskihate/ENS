using Microsoft.AspNetCore.Http;

namespace ENS.Common.Files;
public static class FileExtensions
{
    public static string GetExtension(this IFormFile file,
        bool lowerInvariant = true,
        bool withDot = false)
    {
        var extension = Path.GetExtension(file.FileName);
        extension = lowerInvariant
            ? extension.ToLowerInvariant()
            : extension;
        if (extension.StartsWith('.'))
        {
            extension = withDot
                ? extension
                : extension[1..];
        }

        return extension;
    }
}
