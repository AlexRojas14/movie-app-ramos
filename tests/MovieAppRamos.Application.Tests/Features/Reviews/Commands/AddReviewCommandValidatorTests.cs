using MovieAppRamos.Application.Features.Reviews.Commands.AddReview;

namespace MovieAppRamos.Application.Tests.Features.Reviews.Commands;

public sealed class AddReviewCommandValidatorTests
{
    [Fact]
    public void Validate_ShouldFail_WhenRatingIsInvalid()
    {
        var validator = new AddReviewCommandValidator();
        var command = new AddReviewCommand(Guid.NewGuid(), "Alex", 6, "Too high");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(AddReviewCommand.Rating));
    }
}

