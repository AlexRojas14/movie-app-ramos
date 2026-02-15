using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Abstractions.Persistence;

public interface IReviewRepository
{
    Task<PagedResult<Review>> GetByMovieAsync(
        Guid movieId,
        int page,
        int pageSize,
        string? sort,
        CancellationToken cancellationToken);
}

