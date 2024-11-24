using ENS.Contracts;
using ENS.Serialization;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ENS.Http.Middlewares;
internal class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        switch (exception)
        {
            case ValidationException:
                code = HttpStatusCode.BadRequest;
                break;
        }

        context.Response.ContentType = "Application/json";
        context.Response.StatusCode = (int)code;
        Log.Error("URL: {Method} {DiplayUrl}{Message}{StackTrace}",
            context.Request.Method,
            context.Request.Path,
            $"{Environment.NewLine}{exception.Message}",
            $"{Environment.NewLine}{exception.StackTrace}");

        context.Response.WriteAsync(new ResponseDto()
        {
            IsSuccess = false,
            Message = exception.Message,
        }.ToJson());
    }
}

