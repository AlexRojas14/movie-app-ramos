using Microsoft.EntityFrameworkCore;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Infrastructure.Persistence.Repositories;

public sealed class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _dbContext;

    public ReviewRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Review>> GetByMovieAsync(
        Guid movieId,
        int page,
        int pageSize,
        string? sort,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Reviews
            .AsNoTracking()
            .Where(x => x.MovieId == movieId);

        query = ApplySort(query, sort);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Review>(items, page, pageSize, totalCount);
    }

    private static IQueryable<Review> ApplySort(IQueryable<Review> query, string? sort)
    {
        return sort?.Trim().ToLowerInvariant() switch
        {
            "created_asc" => query.OrderBy(x => x.CreatedAt),
            "rating_desc" => query.OrderByDescending(x => x.Rating).ThenByDescending(x => x.CreatedAt),
            "rating_asc" => query.OrderBy(x => x.Rating).ThenByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };
    }
}

