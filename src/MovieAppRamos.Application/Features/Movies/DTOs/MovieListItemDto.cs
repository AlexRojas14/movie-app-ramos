namespace MovieAppRamos.Application.Features.Movies.DTOs;

public sealed record MovieListItemDto(
    Guid Id,
    string Title,
    DateOnly? ReleaseDate,
    bool IsDisabled,
    DateTime CreatedAt,
    decimal? RatingAverage,
    int ReviewsCount);

