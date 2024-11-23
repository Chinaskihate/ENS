using ENS.Contracts;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace ENS.Tests.Common;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureResponseFailedAsync(
        this HttpResponseMessage message,
        HttpStatusCode expectedCode,
        string? expectedMessage = null)
    {
        message.StatusCode.Should().Be(expectedCode);
        var response = await message.Content.ReadFromJsonAsync<ResponseDto>();
        response.IsSuccess.Should().BeFalse();
        if (expectedMessage != null)
        {
            response.Message.Should().Be(expectedMessage);
        }
    }

    public static async Task EnsureResponseSucceededAsync(
         this HttpResponseMessage message,
         string? expectedMessage = null)
    {
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<ResponseDto>();
        response.IsSuccess.Should().BeTrue();
        if (expectedMessage != null)
        {
            response.Message.Should().Be(expectedMessage);
        }
    }
}
