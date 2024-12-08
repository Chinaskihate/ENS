using ENS.Contracts.NotificationConfiguration.Services;
using ENS.Contracts.NotificationConfiguration.Services.Parsing;
using ENS.NotificationConfiguration.Attributes;
using System.Reflection;

namespace ENS.NotificationConfiguration.Services.Parsing;
public class FileParserFactory : IFileParserFactory
{
    private readonly Dictionary<string, Type> _parsers;

    public FileParserFactory()
    {
        _parsers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IFileParser).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<FileParserAttribute>(),
            })
            .Where(x => x.Attribute != null)
            .ToDictionary(x => x.Attribute!.FileExtension, x => x.Type);
    }

    public IFileParser GetParser(string fileExtension)
    {
        if (!_parsers.TryGetValue(fileExtension, out var parserType))
        {
            throw new NotSupportedException($"File type `{fileExtension}` is not supported");
        }

        return (IFileParser)Activator.CreateInstance(parserType);
    }
}
