using MediatR;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Application.Features.Movies.DTOs;

namespace MovieAppRamos.Application.Features.Movies.Queries.ListMovies;

public sealed class ListMoviesQueryHandler : IRequestHandler<ListMoviesQuery, PagedResult<MovieListItemDto>>
{
    private readonly IMovieRepository _movieRepository;

    public ListMoviesQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public Task<PagedResult<MovieListItemDto>> Handle(ListMoviesQuery request, CancellationToken cancellationToken)
    {
        var query = _movieRepository.Query();

        query = ApplyIsDisabledFilter(query, request.IsDisabled);
        query = ApplySearchFilter(query, request.Search);

        var projectedQuery = query.Select(movie => new MovieListProjection
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseDate = movie.ReleaseDate,
            IsDisabled = movie.IsDisabled,
            CreatedAt = movie.CreatedAt,
            ReviewsCount = movie.Reviews.Count,
            RatingAverage = movie.Reviews.Select(review => (decimal?)review.Rating).Average()
        });

        projectedQuery = ApplyRatingFilters(projectedQuery, request.MinRating, request.MaxRating);
        projectedQuery = ApplySorting(projectedQuery, request.SortBy, request.SortDir);

        var totalCount = projectedQuery.Count();
        var pageItems = projectedQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(item => new MovieListItemDto(
                item.Id,
                item.Title,
                item.ReleaseDate,
                item.IsDisabled,
                item.CreatedAt,
                item.RatingAverage,
                item.ReviewsCount))
            .ToList();

        return Task.FromResult(new PagedResult<MovieListItemDto>(
            pageItems,
            request.Page,
            request.PageSize,
            totalCount));
    }

    private static IQueryable<MovieAppRamos.Domain.Entities.Movie> ApplyIsDisabledFilter(
        IQueryable<MovieAppRamos.Domain.Entities.Movie> query,
        bool? isDisabled)
    {
        if (isDisabled.HasValue)
        {
            return query.Where(movie => movie.IsDisabled == isDisabled.Value);
        }

        return query.Where(movie => !movie.IsDisabled);
    }

    private static IQueryable<MovieAppRamos.Domain.Entities.Movie> ApplySearchFilter(
        IQueryable<MovieAppRamos.Domain.Entities.Movie> query,
        string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var normalizedSearch = search.Trim().ToLower();
        return query.Where(movie => movie.Title.ToLower().Contains(normalizedSearch));
    }

    private static IQueryable<MovieListProjection> ApplyRatingFilters(
        IQueryable<MovieListProjection> query,
        decimal? minRating,
        decimal? maxRating)
    {
        if (minRating.HasValue)
        {
            query = query.Where(movie => movie.RatingAverage.HasValue && movie.RatingAverage.Value >= minRating.Value);
        }

        if (maxRating.HasValue)
        {
            query = query.Where(movie => movie.RatingAverage.HasValue && movie.RatingAverage.Value <= maxRating.Value);
        }

        return query;
    }

    private static IQueryable<MovieListProjection> ApplySorting(
        IQueryable<MovieListProjection> query,
        MovieSortBy sortBy,
        SortDirection sortDir)
    {
        return (sortBy, sortDir) switch
        {
            (MovieSortBy.Title, SortDirection.Asc) => query.OrderBy(movie => movie.Title),
            (MovieSortBy.Title, SortDirection.Desc) => query.OrderByDescending(movie => movie.Title),

            (MovieSortBy.ReleaseDate, SortDirection.Asc) => query.OrderBy(movie => movie.ReleaseDate),
            (MovieSortBy.ReleaseDate, SortDirection.Desc) => query.OrderByDescending(movie => movie.ReleaseDate),

            (MovieSortBy.CreatedAt, SortDirection.Asc) => query.OrderBy(movie => movie.CreatedAt),
            (MovieSortBy.CreatedAt, SortDirection.Desc) => query.OrderByDescending(movie => movie.CreatedAt),

            (MovieSortBy.RatingAvg, SortDirection.Asc) => query
                .OrderBy(movie => movie.RatingAverage ?? decimal.MaxValue)
                .ThenBy(movie => movie.Title),

            _ => query
                .OrderByDescending(movie => movie.RatingAverage ?? decimal.MinValue)
                .ThenBy(movie => movie.Title)
        };
    }

    private sealed class MovieListProjection
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public DateOnly? ReleaseDate { get; init; }
        public bool IsDisabled { get; init; }
        public DateTime CreatedAt { get; init; }
        public int ReviewsCount { get; init; }
        public decimal? RatingAverage { get; init; }
    }
}

