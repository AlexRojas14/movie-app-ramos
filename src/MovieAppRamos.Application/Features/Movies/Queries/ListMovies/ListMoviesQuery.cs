using MediatR;
using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Application.Features.Movies.DTOs;

namespace MovieAppRamos.Application.Features.Movies.Queries.ListMovies;

public sealed record ListMoviesQuery(
    string? Search = null,
    decimal? MinRating = null,
    decimal? MaxRating = null,
    bool? IsDisabled = false,
    MovieSortBy SortBy = MovieSortBy.CreatedAt,
    SortDirection SortDir = SortDirection.Desc,
    int Page = 1,
    int PageSize = 10) : IRequest<PagedResult<MovieListItemDto>>;

