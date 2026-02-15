using Moq;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Features.Movies.Commands.CreateMovie;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Tests.Features.Movies.Commands;

public sealed class CreateMovieCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSaveAndReturnDto()
    {
        var movieRepository = new Mock<IMovieRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        Movie? capturedMovie = null;

        movieRepository
            .Setup(x => x.AddAsync(It.IsAny<Movie>(), It.IsAny<CancellationToken>()))
            .Callback<Movie, CancellationToken>((movie, _) => capturedMovie = movie)
            .Returns(Task.CompletedTask);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateMovieCommandHandler(movieRepository.Object, unitOfWork.Object);
        var command = new CreateMovieCommand("The Matrix", "Sci-fi", new DateOnly(1999, 3, 31));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(capturedMovie);
        Assert.Equal(capturedMovie!.Id, result.Id);
        Assert.Equal("The Matrix", result.Title);
        movieRepository.Verify(x => x.AddAsync(It.IsAny<Movie>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

