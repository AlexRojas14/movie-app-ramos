using Microsoft.EntityFrameworkCore;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplySoftDisableForMovies();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplySoftDisableForMovies();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplySoftDisableForMovies()
    {
        var deletedMovies = ChangeTracker
            .Entries<Movie>()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var entry in deletedMovies)
        {
            entry.State = EntityState.Modified;
            entry.Entity.Disable();
        }
    }
}

