using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieAppRamos.Api.Contracts.Movies;
using MovieAppRamos.Application.Common.Models;
using MovieAppRamos.Application.Features.Movies.Commands.CreateMovie;
using MovieAppRamos.Application.Features.Movies.Commands.DisableMovie;
using MovieAppRamos.Application.Features.Movies.DTOs;
using MovieAppRamos.Application.Features.Movies.Queries.GetMovie;
using MovieAppRamos.Application.Features.Movies.Queries.ListMovies;
using MovieAppRamos.Application.Features.Reviews.Commands.AddReview;
using MovieAppRamos.Application.Features.Reviews.DTOs;
using MovieAppRamos.Application.Features.Reviews.Queries.ListMovieReviews;

namespace MovieAppRamos.Api.Controllers;

[ApiController]
[Route("movies")]
public sealed class MoviesController : ControllerBase
{
    private readonly ISender _sender;

    public MoviesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<MovieDto>> CreateMovie(
        [FromBody] CreateMovieRequest request,
        CancellationToken cancellationToken)
    {
        var createdMovie = await _sender.Send(
            new CreateMovieCommand(request.Title, request.Description, request.ReleaseDate),
            cancellationToken);

        return CreatedAtAction(nameof(GetMovieById), new { id = createdMovie.Id }, createdMovie);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MovieDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDetailsDto>> GetMovieById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var movie = await _sender.Send(new GetMovieQuery(id), cancellationToken);
        return Ok(movie);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MovieListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<MovieListItemDto>>> ListMovies(
        [FromQuery] ListMoviesRequest request,
        CancellationToken cancellationToken)
    {
        var movies = await _sender.Send(
            new ListMoviesQuery(
                request.Search,
                request.MinRating,
                request.MaxRating,
                request.IsDisabled,
                request.SortBy,
                request.SortDir,
                request.Page,
                request.PageSize),
            cancellationToken);

        return Ok(movies);
    }

    [HttpPost("{id:guid}/reviews")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ReviewDto>> AddReview(
        Guid id,
        [FromBody] AddReviewRequest request,
        CancellationToken cancellationToken)
    {
        var review = await _sender.Send(
            new AddReviewCommand(id, request.AuthorName, request.Rating, request.Comment),
            cancellationToken);

        return Created($"/movies/{id}/reviews/{review.Id}", review);
    }

    [HttpGet("{id:guid}/reviews")]
    [ProducesResponseType(typeof(PagedResult<ReviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResult<ReviewDto>>> ListMovieReviews(
        Guid id,
        [FromQuery] ListMovieReviewsRequest request,
        CancellationToken cancellationToken)
    {
        var reviews = await _sender.Send(
            new ListMovieReviewsQuery(id, request.Page, request.PageSize, request.Sort),
            cancellationToken);

        return Ok(reviews);
    }

    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DisableMovie(Guid id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DisableMovieCommand(id), cancellationToken);
        return NoContent();
    }
}
