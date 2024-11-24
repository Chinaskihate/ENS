using ENS.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ENS.Persistence.Helpers;
public static class MigrationHelper
{
    public static IServiceCollection AddNotificationDbContextFactory(this IServiceCollection services, string connectionString)
    {
        Log.Warning($"Connect to db: {connectionString}");
        return services.AddDbContextFactory<NotificationContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString)
                .LogTo(Log.Logger.Information, Microsoft.Extensions.Logging.LogLevel.Information, null);
        });
    }

    public static void ApplyNotificationMigration(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var db = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<NotificationContext>>()
            .CreateDbContext();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }
    }
}
