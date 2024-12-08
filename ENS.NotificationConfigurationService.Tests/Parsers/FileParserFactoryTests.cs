using ENS.NotificationConfiguration.Services.Parsing;
using FluentAssertions;

namespace ENS.NotificationConfigurationService.Tests.Parsers;
[TestFixture]
public class FileParserFactoryTests
{
    private FileParserFactory _fileParserFactory;

    [OneTimeSetUp]
    public void SetUp()
    {
        _fileParserFactory = new FileParserFactory();
    }

    [TestCase("csv", typeof(CsvFileParser))]
    [TestCase("json", typeof(JsonFileParser))]
    public void GetParser_Extension_ReturnsCorrectParserForExtension(string extension, Type type)
    {
        var parser = _fileParserFactory.GetParser(extension);

        parser.Should().BeOfType(type);
    }
}
