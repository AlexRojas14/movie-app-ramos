using MediatR;
using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Application.Features.Reviews.DTOs;

namespace MovieAppRamos.Application.Features.Reviews.Queries.ListMovieReviews;

public sealed record ListMovieReviewsQuery(
    Guid MovieId,
    int Page = 1,
    int PageSize = 10,
    string? Sort = null) : IRequest<PagedResult<ReviewDto>>;

