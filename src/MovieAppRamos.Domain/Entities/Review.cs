using MovieAppRamos.Domain.Exceptions;

namespace MovieAppRamos.Domain.Entities;

public sealed class Review
{
    public const int MinRating = 1;
    public const int MaxRating = 5;
    public const int MaxAuthorNameLength = 120;
    public const int MaxCommentLength = 2000;

    private Review()
    {
    }

    private Review(Guid id, Guid movieId, string authorName, int rating, string? comment, DateTime createdAt)
    {
        Id = id;
        MovieId = movieId;
        AuthorName = authorName;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid MovieId { get; private set; }
    public string AuthorName { get; private set; } = string.Empty;
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal static Review Create(Guid movieId, string authorName, int rating, string? comment)
    {
        if (movieId == Guid.Empty)
        {
            throw new DomainException("MovieId cannot be empty.");
        }

        var normalizedAuthorName = (authorName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedAuthorName))
        {
            throw new DomainException("Author name is required.");
        }

        if (normalizedAuthorName.Length > MaxAuthorNameLength)
        {
            throw new DomainException($"Author name cannot exceed {MaxAuthorNameLength} characters.");
        }

        if (rating < MinRating || rating > MaxRating)
        {
            throw new DomainException($"Rating must be between {MinRating} and {MaxRating}.");
        }

        var normalizedComment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
        if (normalizedComment is not null && normalizedComment.Length > MaxCommentLength)
        {
            throw new DomainException($"Comment cannot exceed {MaxCommentLength} characters.");
        }

        return new Review(Guid.NewGuid(), movieId, normalizedAuthorName, rating, normalizedComment, DateTime.UtcNow);
    }
}

