using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Abstractions.Persistence;

public interface IMovieRepository
{
    IQueryable<Movie> Query();
    Task AddAsync(Movie movie, CancellationToken cancellationToken);
    Task<Movie?> GetByIdAsync(Guid movieId, CancellationToken cancellationToken);
    Task<Movie?> GetByIdWithReviewsAsync(Guid movieId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid movieId, CancellationToken cancellationToken);
}

