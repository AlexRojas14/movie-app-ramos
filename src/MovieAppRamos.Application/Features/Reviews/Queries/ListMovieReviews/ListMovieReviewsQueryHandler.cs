using MediatR;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Common.Exceptions;
using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Application.Features.Reviews.DTOs;
using MovieAppRamos.Application.Features.Reviews.Mappings;

namespace MovieAppRamos.Application.Features.Reviews.Queries.ListMovieReviews;

public sealed class ListMovieReviewsQueryHandler : IRequestHandler<ListMovieReviewsQuery, PagedResult<ReviewDto>>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IReviewRepository _reviewRepository;

    public ListMovieReviewsQueryHandler(IMovieRepository movieRepository, IReviewRepository reviewRepository)
    {
        _movieRepository = movieRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<PagedResult<ReviewDto>> Handle(ListMovieReviewsQuery request, CancellationToken cancellationToken)
    {
        var movieExists = await _movieRepository.ExistsAsync(request.MovieId, cancellationToken);
        if (!movieExists)
        {
            throw new NotFoundException("Movie", request.MovieId.ToString());
        }

        var page = await _reviewRepository.GetByMovieAsync(
            request.MovieId,
            request.Page,
            request.PageSize,
            request.Sort,
            cancellationToken);

        var items = page.Items.Select(x => x.ToDto()).ToList();
        return new PagedResult<ReviewDto>(items, page.Page, page.PageSize, page.TotalCount);
    }
}

