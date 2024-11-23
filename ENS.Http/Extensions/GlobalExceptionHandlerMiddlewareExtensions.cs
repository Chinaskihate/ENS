using ENS.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ENS.Http.Extensions;
public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder) =>
        builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
