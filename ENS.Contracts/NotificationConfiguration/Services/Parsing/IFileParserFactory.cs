namespace ENS.Contracts.NotificationConfiguration.Services.Parsing;
public interface IFileParserFactory
{
    public IFileParser GetParser(string fileExtension);
}
