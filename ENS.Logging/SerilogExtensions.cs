using Microsoft.Extensions.Hosting;
using Serilog;

namespace ENS.Logging;

public static class SerilogExtensions
{
    public static void SerilogTo(this IHostBuilder builder, SerilogOutputType outputType)
    {
        builder.UseSerilog((ctx, lc) =>
        {
            var template = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
            LoggerConfiguration lcWithOutput = outputType switch
            {
                SerilogOutputType.Console => lc.WriteTo.Console(outputTemplate: template),
                _ => throw new ArgumentException("Unknown output type"),
            };

            lcWithOutput.Enrich.FromLogContext()
                .ReadFrom.Configuration(ctx.Configuration);
        });
    }
}

public enum SerilogOutputType
{
    Console
}