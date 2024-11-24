using Microsoft.AspNetCore.Http.Features;
using ENS.Http.Extensions;
using ENS.Logging;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.NotificationConfiguration.Services.Validation;
using ENS.NotificationConfiguration.Services;
using Microsoft.AspNetCore.Mvc;
using ENS.Persistence.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.SerilogTo(SerilogOutputType.Console);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers();

builder.Services.AddNotificationDbContextFactory("Server=localhost;Port=5432;Database=NotificationConfiguration;User Id=postgres;Password=sapwd;");

builder.Services.AddScoped<IFileValidationService, FileValidationService>(sp =>
    new FileValidationService(new FileValidationSettings
    {
        MaxSizeInBytes = 1024,
        AllowedExtensions = ["csv", "xlsx"]
    }));

builder.Services.AddScoped<INotificationConfigurationService, NotificationConfigurationService>(sp =>
    new NotificationConfigurationService(sp.GetRequiredService<IFileValidationService>()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 268435456; // Set file size limit to 256 MB (adjust as needed)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionHandler();

app.UseAuthorization();

app.MapControllers();

MigrationHelper.ApplyNotificationMigration(app.Services);

app.Run();

// for tests
public partial class Program { }