namespace MovieAppRamos.Api.Contracts.Movies;

public sealed class ListMovieReviewsRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Sort { get; init; }
}
