using MediatR;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Features.Movies.DTOs;
using MovieAppRamos.Application.Features.Movies.Mappings;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Features.Movies.Commands.CreateMovie;

public sealed class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MovieDto> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = Movie.Create(request.Title, request.Description, request.ReleaseDate);
        await _movieRepository.AddAsync(movie, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return movie.ToDto();
    }
}

