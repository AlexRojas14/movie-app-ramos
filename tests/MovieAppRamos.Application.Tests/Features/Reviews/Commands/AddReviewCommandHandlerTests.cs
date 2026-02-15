using Moq;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Features.Reviews.Commands.AddReview;
using MovieAppRamos.Domain.Entities;
using MovieAppRamos.Domain.Exceptions;

namespace MovieAppRamos.Application.Tests.Features.Reviews.Commands;

public sealed class AddReviewCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrow_WhenMovieIsDisabled()
    {
        var movie = Movie.Create("Disabled movie");
        movie.Disable();

        var movieRepository = new Mock<IMovieRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        movieRepository
            .Setup(x => x.GetByIdAsync(movie.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);

        var handler = new AddReviewCommandHandler(movieRepository.Object, unitOfWork.Object);
        var command = new AddReviewCommand(movie.Id, "Alex", 5, "Great");

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

