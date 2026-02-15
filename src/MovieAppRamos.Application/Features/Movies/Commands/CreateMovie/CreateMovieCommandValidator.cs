using FluentValidation;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Features.Movies.Commands.CreateMovie;

public sealed class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Movie.MaxTitleLength);

        RuleFor(x => x.Description)
            .MaximumLength(Movie.MaxDescriptionLength);
    }
}

