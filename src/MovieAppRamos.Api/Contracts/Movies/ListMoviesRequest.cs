using MovieAppRamos.Application.Features.Movies.Queries.ListMovies;

namespace MovieAppRamos.Api.Contracts.Movies;

public sealed class ListMoviesRequest
{
    public string? Search { get; init; }
    public decimal? MinRating { get; init; }
    public decimal? MaxRating { get; init; }
    public bool? IsDisabled { get; init; } = false;
    public MovieSortBy SortBy { get; init; } = MovieSortBy.CreatedAt;
    public SortDirection SortDir { get; init; } = SortDirection.Desc;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
