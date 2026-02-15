using FluentValidation;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Features.Reviews.Commands.AddReview;

public sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.AuthorName)
            .NotEmpty()
            .MaximumLength(Review.MaxAuthorNameLength);

        RuleFor(x => x.Rating)
            .InclusiveBetween(Review.MinRating, Review.MaxRating);

        RuleFor(x => x.Comment)
            .MaximumLength(Review.MaxCommentLength);
    }
}

