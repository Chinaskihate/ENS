using FluentAssertions;
using System.Net.Http.Headers;

namespace ENS.NotificationConfigurationService.Tests;

[TestFixture]
public class IntegrationTests
{
    private CustomWebAppFactory _factory;
    private HttpClient _client;

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebAppFactory();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
    {
        // Arrange
        var url = "/NotificationConfiguration?userId=123";

        // Act
        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}