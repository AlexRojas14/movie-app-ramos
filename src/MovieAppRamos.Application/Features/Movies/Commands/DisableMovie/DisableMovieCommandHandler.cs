using MediatR;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Common.Exceptions;

namespace MovieAppRamos.Application.Features.Movies.Commands.DisableMovie;

public sealed class DisableMovieCommandHandler : IRequestHandler<DisableMovieCommand>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DisableMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DisableMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie is null)
        {
            throw new NotFoundException("Movie", request.MovieId.ToString());
        }

        movie.Disable();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

