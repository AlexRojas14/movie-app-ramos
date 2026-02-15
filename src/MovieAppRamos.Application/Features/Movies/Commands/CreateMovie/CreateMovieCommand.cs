using MediatR;
using MovieAppRamos.Application.Features.Movies.DTOs;

namespace MovieAppRamos.Application.Features.Movies.Commands.CreateMovie;

public sealed record CreateMovieCommand(
    string Title,
    string? Description,
    DateOnly? ReleaseDate) : IRequest<MovieDto>;

