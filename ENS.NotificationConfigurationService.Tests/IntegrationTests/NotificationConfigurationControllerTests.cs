using ENS.Resources.Errors;
using ENS.Resources.Messages;
using ENS.Tests.Common;
using System.Net;
using System.Net.Http.Headers;

namespace ENS.NotificationConfigurationService.Tests.IntegrationTests;

[TestFixture]
public class NotificationConfigurationControllerTests : BaseIntegrationTest
{
    protected override string ControllerRoute => "NotificationConfiguration";

    [Test]
    public async Task UploadFile_ValidCsvFile_ReturnsSuccess()
    {
        var content = GetContent("test.csv", "header");

        var response = await _client.PostAsync(ControllerRoute, content);

        await response.EnsureResponseSucceededAsync(Messages.FileUploaded);
    }

    [TestCase("text/plain")]
    [TestCase("text/csv")]
    public async Task UploadFile_InvalidFileType_ReturnsBadRequest(string mediaType)
    {
        var content = GetContent("test.txt", "test", mediaType);

        var response = await _client.PostAsync(ControllerRoute, content);

        await response.EnsureResponseFailedAsync(HttpStatusCode.BadRequest, Errors.UnsupportedExtension);
    }

    [Test]
    public async Task UploadFile_NoFile_ReturnsBadRequest()
    {
        var content = new MultipartFormDataContent();

        var response = await _client.PostAsync(ControllerRoute, content);

        await response.EnsureResponseFailedAsync(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task UploadFile_EmptyFile_ReturnsBadRequest()
    {
        var content = GetContent("test.csv", string.Empty);

        var response = await _client.PostAsync(ControllerRoute, content);

        await response.EnsureResponseFailedAsync(HttpStatusCode.BadRequest, Errors.EmptyFile);
    }

    private static MultipartFormDataContent GetContent(
        string fileName,
        string fileContent,
        string mediaType = "text/csv")
    {
        var fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

        var content = new MultipartFormDataContent();
        var fileStreamContent = new ByteArrayContent(fileBytes);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

        content.Add(fileStreamContent, "file", fileName);

        return content;
    }
}