using Microsoft.EntityFrameworkCore;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Infrastructure.Persistence.Repositories;

public sealed class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _dbContext;

    public MovieRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Movie> Query()
    {
        return _dbContext.Movies.AsNoTracking();
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken)
    {
        await _dbContext.Movies.AddAsync(movie, cancellationToken);
    }

    public Task<Movie?> GetByIdAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == movieId, cancellationToken);
    }

    public Task<Movie?> GetByIdWithReviewsAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return _dbContext.Movies
            .AsNoTracking()
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == movieId, cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return _dbContext.Movies.AnyAsync(x => x.Id == movieId, cancellationToken);
    }
}

