using FluentValidation;

namespace MovieAppRamos.Application.Features.Movies.Commands.DisableMovie;

public sealed class DisableMovieCommandValidator : AbstractValidator<DisableMovieCommand>
{
    public DisableMovieCommandValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEqual(Guid.Empty);
    }
}

