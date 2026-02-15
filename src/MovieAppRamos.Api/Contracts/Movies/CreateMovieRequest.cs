namespace MovieAppRamos.Api.Contracts.Movies;

public sealed record CreateMovieRequest(
    string Title,
    string? Description,
    DateOnly? ReleaseDate);
