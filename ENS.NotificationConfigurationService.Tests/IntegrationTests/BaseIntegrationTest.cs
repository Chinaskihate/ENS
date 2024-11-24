using System.Net.Http.Headers;

namespace ENS.NotificationConfigurationService.Tests.IntegrationTests;

[TestFixture]
public abstract class BaseIntegrationTest
{
    private CustomWebAppFactory _factory;
    protected HttpClient _client;

    protected abstract string ControllerRoute { get; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new CustomWebAppFactory();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
