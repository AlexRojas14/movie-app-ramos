using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MovieAppRamos.Infrastructure.Persistence;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("MOVIEAPPRAMOS_SQLSERVER")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=MovieAppRamosDb;Trusted_Connection=True;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure();
        });

        return new AppDbContext(optionsBuilder.Options);
    }
}

