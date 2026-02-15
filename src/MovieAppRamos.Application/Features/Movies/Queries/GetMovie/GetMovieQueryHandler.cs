using MediatR;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Common.Exceptions;
using MovieAppRamos.Application.Features.Movies.DTOs;
using MovieAppRamos.Application.Features.Movies.Mappings;

namespace MovieAppRamos.Application.Features.Movies.Queries.GetMovie;

public sealed class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, MovieDetailsDto>
{
    private readonly IMovieRepository _movieRepository;

    public GetMovieQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<MovieDetailsDto> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdWithReviewsAsync(request.MovieId, cancellationToken);
        if (movie is null)
        {
            throw new NotFoundException("Movie", request.MovieId.ToString());
        }

        return movie.ToDetailsDto();
    }
}

