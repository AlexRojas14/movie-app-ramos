using MediatR;

namespace MovieAppRamos.Application.Features.Movies.Commands.DisableMovie;

public sealed record DisableMovieCommand(Guid MovieId) : IRequest;

