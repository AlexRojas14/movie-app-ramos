using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Infrastructure.Persistence;
using MovieAppRamos.Infrastructure.Persistence.Seeding;
using MovieAppRamos.Infrastructure.Persistence.Repositories;

namespace MovieAppRamos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'SqlServer' is not configured.");
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                sql.EnableRetryOnFailure();
            });
        });

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<DevelopmentDataSeeder>();

        return services;
    }
}

