using FluentValidation;

namespace MovieAppRamos.Application.Features.Reviews.Queries.ListMovieReviews;

public sealed class ListMovieReviewsQueryValidator : AbstractValidator<ListMovieReviewsQuery>
{
    public ListMovieReviewsQueryValidator()
    {
        RuleFor(x => x.MovieId).NotEqual(Guid.Empty);
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

