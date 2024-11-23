using Microsoft.AspNetCore.Http.Features;
using ENS.Http.Extensions;
using ENS.Logging;
using ENS.Contracts.NotificationConfiguration.Services;
using ENS.NotificationConfiguration.Services.Validation;
using ENS.NotificationConfiguration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.SerilogTo(SerilogOutputType.Console);
builder.Services.AddControllers();

builder.Services.AddScoped<IFileValidationService, FileValidationService>(sp =>
    new FileValidationService(new FileValidationSettings
    {
        MaxSizeInBytes = 1024,
        AllowedExtensions = ["csv", "xlsx"]
    }));

builder.Services.AddScoped<INotificationConfigurationService, NotificationConfigurationService>(sp =>
    new NotificationConfigurationService(sp.GetRequiredService<IFileValidationService>()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.Run();

public partial class Program { }