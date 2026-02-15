using MovieAppRamos.Application.Features.Movies.Commands.CreateMovie;

namespace MovieAppRamos.Application.Tests.Features.Movies.Commands;

public sealed class CreateMovieCommandValidatorTests
{
    [Fact]
    public void Validate_ShouldFail_WhenTitleIsRequired()
    {
        var validator = new CreateMovieCommandValidator();
        var command = new CreateMovieCommand(string.Empty, "Some description", DateOnly.FromDateTime(DateTime.UtcNow));

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(CreateMovieCommand.Title));
    }
}

