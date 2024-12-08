using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ENS.Contracts.NotificationConfiguration.Models;
using ENS.NotificationConfiguration.Services.Parsing;
using FluentAssertions;
using System.Globalization;
using System.Text;

namespace ENS.NotificationConfigurationService.Tests.Parsers;
[TestFixture]
public class CsvFileParserTests
{
    private CsvFileParser _csvParser;

    [SetUp]
    public void SetUp()
    {
        _csvParser = new CsvFileParser();
    }

    [Test]
    public void Parse_ValidCsv_ReturnsExpectedResults()
    {
        // Arrange
        var dtos = new[]
        {
            new NotificationConfigurationDto
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Channels = [Channel.Email],
                UserIdInChannels = ["1"],
                TemplateId = "101",
                TemplateMessage = "Hello, John",
                TemplateChannels = [Channel.Email]
            },
            new NotificationConfigurationDto
            {
                UserId = null,
                FirstName = "Jane",
                LastName = "Smith",
                Channels = null,
                UserIdInChannels = ["2"],
                TemplateId = "102",
                TemplateMessage = "Hi, Jane",
                TemplateChannels = null
            }
        };
        using var stream = GenerateStreamFromDtos(dtos);

        // Act
        var result = _csvParser.Parse(stream);

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().BeEquivalentTo(dtos[0]);
        result[1].Should().BeEquivalentTo(dtos[1]);
    }

    [Test]
    public void Parse_EmptyCsv_ReturnsEmptyList()
    {
        // Arrange
        using var stream = GenerateStreamFromDtos([]);

        // Act
        var result = _csvParser.Parse(stream);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public void Parse_MissingFields_ParsesCorrectly()
    {
        // Arrange
        var dtos = new[]
        {
            new NotificationConfigurationDto
            {
                FirstName = "John",
                Channels = [Channel.Email],
                TemplateId = "101",
            },
            new NotificationConfigurationDto
            {
                UserId = Guid.NewGuid(),
                FirstName = "Jane",
            }
        };
        using var stream = GenerateStreamFromDtos(dtos);

        // Act
        var result = _csvParser.Parse(stream);

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().BeEquivalentTo(dtos[0]);
        result[1].Should().BeEquivalentTo(dtos[1]);
    }

    [Test]
    public void Parse_InvalidCsv_ThrowsException()
    {
        // Arrange
        var csvContent = @"
user_id;first_name;last_name;channels
1;John;Doe
";
        using var stream = GenerateStreamFromString(csvContent);

        // Act & Assert
        Assert.Throws<TypeConverterException>(() => _csvParser.Parse(stream));
    }

    private MemoryStream GenerateStreamFromDtos(NotificationConfigurationDto[] dtos)
    {
        var stream = new MemoryStream();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };
        using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        using var csv = new CsvWriter(writer, config, leaveOpen: true);

        csv.Context.RegisterClassMap<NotificationConfigurationDtoMap>();

            csv.WriteRecords(dtos);

        writer.Flush();
        stream.Position = 0; // Reset the stream position to the beginning
        return stream;
    }

    private Stream GenerateStreamFromString(string content)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0; // Reset the stream position to the beginning
        return stream;
    }
}
