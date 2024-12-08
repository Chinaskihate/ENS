using ENS.Contracts.NotificationConfiguration.Models;
using ENS.NotificationConfiguration.Services.Parsing;
using ENS.Serialization;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace ENS.NotificationConfigurationService.Tests.Parsers;
[TestFixture]
public class JsonParserTests
{
    private JsonFileParser _jsonParser;

    [SetUp]
    public void SetUp()
    {
        _jsonParser = new JsonFileParser();
    }

    [Test]
    public void Parse_ValidJson_ReturnsExpectedResults()
    {
        var dtos = new[]
        {
            new NotificationConfigurationDto
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Channels = new[] { Channel.Email },
                UserIdInChannels = new[] { "1" },
                TemplateId = "101",
                TemplateMessage = "Hello, John",
                TemplateChannels = new[] { Channel.Email }
            },
            new NotificationConfigurationDto
            {
                UserId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Channels = null,
                UserIdInChannels = new[] { "2" },
                TemplateId = "102",
                TemplateMessage = "Hi, Jane",
                TemplateChannels = null
            }
        };

        using var stream = GenerateStreamFromDtos(dtos);

        var result = _jsonParser.Parse(stream);

        result.Should().HaveCount(2);
        result[0].Should().BeEquivalentTo(dtos[0]);
        result[1].Should().BeEquivalentTo(dtos[1]);
    }

    [Test]
    public void Parse_EmptyJson_ReturnsEmptyList()
    {
        using var stream = GenerateStreamFromDtos([]);

        var result = _jsonParser.Parse(stream);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public void Parse_InvalidJson_ThrowsJsonException()
    {
        var invalidJsonContent = "{ invalid json content ";
        using var stream = GenerateStreamFromJson(invalidJsonContent);

        var act = () => _jsonParser.Parse(stream);

        act.Should().Throw<JsonReaderException>();
    }

    private MemoryStream GenerateStreamFromDtos(NotificationConfigurationDto[] dtos)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(dtos.ToJson());
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    private MemoryStream GenerateStreamFromJson(string jsonContent)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(jsonContent);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
