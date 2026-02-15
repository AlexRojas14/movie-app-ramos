namespace MovieAppRamos.Api.Contracts.Movies;

public sealed record AddReviewRequest(
    string AuthorName,
    int Rating,
    string? Comment);
