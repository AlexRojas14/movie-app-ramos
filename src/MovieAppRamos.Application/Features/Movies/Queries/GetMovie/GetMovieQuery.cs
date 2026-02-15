using MediatR;
using MovieAppRamos.Application.Features.Movies.DTOs;

namespace MovieAppRamos.Application.Features.Movies.Queries.GetMovie;

public sealed record GetMovieQuery(Guid MovieId) : IRequest<MovieDetailsDto>;

