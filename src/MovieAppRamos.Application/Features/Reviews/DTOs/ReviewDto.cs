namespace MovieAppRamos.Application.Features.Reviews.DTOs;

public sealed record ReviewDto(
    Guid Id,
    Guid MovieId,
    string AuthorName,
    int Rating,
    string? Comment,
    DateTime CreatedAt);

