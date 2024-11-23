using FluentAssertions;
using System.Net;
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
    public async Task Post_UploadFileAsync_ValidCsvFile_ReturnsSuccess()
    {
        // Arrange
        var url = "/NotificationConfiguration"; // Ensure this matches your route
        var fileName = "test.csv";
        var fileContent = "header1,header2\nvalue1,value2"; // Example CSV content
        var fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

        var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileBytes);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

        content.Add(fileStreamContent, "file", fileName);

        // Act
        var response = await _client.PostAsync(url, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("File uploaded successfully");
    }

    [Test]
    public async Task Post_UploadFileAsync_InvalidFileType_ReturnsBadRequest()
    {
        // Arrange
        var url = "/NotificationConfiguration"; // Ensure this matches your route
        var fileName = "test.txt"; // Invalid file type
        var fileContent = "This is a text file."; // Example content
        var fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

        var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileBytes);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        content.Add(fileStreamContent, "file", fileName);

        // Act
        var response = await _client.PostAsync(url, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("Only .csv and .xlsx files are allowed");
    }

    [Test]
    public async Task Post_UploadFileAsync_NoFile_ReturnsBadRequest()
    {
        // Arrange
        var url = "/NotificationConfiguration"; // Ensure this matches your route
        var content = new MultipartFormDataContent(); // No file added

        // Act
        var response = await _client.PostAsync(url, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseString = await response.Content.ReadAsStringAsync();
    }

    [Test]
    public async Task Post_UploadFileAsync_EmptyFile_ReturnsBadRequest()
    {
        // Arrange
        var url = "/NotificationConfiguration"; // Ensure this matches your route
        var fileName = "test.csv";
        var fileContent = string.Empty;
        var fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

        var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileBytes);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

        content.Add(fileStreamContent, "file", fileName);

        // Act
        var response = await _client.PostAsync(url, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("No file uploaded");
    }
}