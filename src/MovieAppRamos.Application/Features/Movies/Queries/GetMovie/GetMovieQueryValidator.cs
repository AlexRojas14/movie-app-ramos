using FluentValidation;

namespace MovieAppRamos.Application.Features.Movies.Queries.GetMovie;

public sealed class GetMovieQueryValidator : AbstractValidator<GetMovieQuery>
{
    public GetMovieQueryValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEqual(Guid.Empty);
    }
}

