namespace MovieAppRamos.Application.Features.Movies.DTOs;

public sealed record MovieDetailsDto(
    Guid Id,
    string Title,
    string? Description,
    DateOnly? ReleaseDate,
    bool IsDisabled,
    DateTime CreatedAt,
    decimal? AverageRating,
    int ReviewsCount);

