using Microsoft.Extensions.DependencyInjection;

namespace MovieAppRamos.Infrastructure.Persistence.Seeding;

public static class SeedingExtensions
{
    public static async Task SeedDevelopmentDataAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DevelopmentDataSeeder>();
        await seeder.SeedAsync(cancellationToken);
    }
}
