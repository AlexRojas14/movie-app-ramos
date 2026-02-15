using MediatR;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Common.Exceptions;
using MovieAppRamos.Application.Features.Reviews.DTOs;
using MovieAppRamos.Application.Features.Reviews.Mappings;

namespace MovieAppRamos.Application.Features.Reviews.Commands.AddReview;

public sealed class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, ReviewDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddReviewCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewDto> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie is null)
        {
            throw new NotFoundException("Movie", request.MovieId.ToString());
        }

        var review = movie.AddReview(request.AuthorName, request.Rating, request.Comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return review.ToDto();
    }
}

