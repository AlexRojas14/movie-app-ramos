namespace MovieAppRamos.Application.Features.Movies.DTOs;

public sealed record MovieDto(
    Guid Id,
    string Title,
    string? Description,
    DateOnly? ReleaseDate,
    bool IsDisabled,
    DateTime CreatedAt);

