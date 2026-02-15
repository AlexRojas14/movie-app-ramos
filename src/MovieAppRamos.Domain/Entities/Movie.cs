using MovieAppRamos.Domain.Exceptions;

namespace MovieAppRamos.Domain.Entities;

public sealed class Movie
{
    public const int MaxTitleLength = 200;
    public const int MaxDescriptionLength = 4000;

    private readonly List<Review> _reviews = [];

    private Movie()
    {
    }

    private Movie(Guid id, string title, string? description, DateOnly? releaseDate, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        ReleaseDate = releaseDate;
        IsDisabled = false;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateOnly? ReleaseDate { get; private set; }
    public bool IsDisabled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Review> Reviews => _reviews;

    public static Movie Create(string title, string? description = null, DateOnly? releaseDate = null)
    {
        var normalizedTitle = NormalizeTitle(title);
        var normalizedDescription = NormalizeDescription(description);

        return new Movie(Guid.NewGuid(), normalizedTitle, normalizedDescription, releaseDate, DateTime.UtcNow);
    }

    public void Disable()
    {
        IsDisabled = true;
    }

    public Review AddReview(string authorName, int rating, string? comment = null)
    {
        if (IsDisabled)
        {
            throw new DomainException("Cannot add reviews to a disabled movie.");
        }

        var review = Review.Create(Id, authorName, rating, comment);
        _reviews.Add(review);

        return review;
    }

    private static string NormalizeTitle(string title)
    {
        var normalizedTitle = (title ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedTitle))
        {
            throw new DomainException("Movie title is required.");
        }

        if (normalizedTitle.Length > MaxTitleLength)
        {
            throw new DomainException($"Movie title cannot exceed {MaxTitleLength} characters.");
        }

        return normalizedTitle;
    }

    private static string? NormalizeDescription(string? description)
    {
        var normalizedDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        if (normalizedDescription is not null && normalizedDescription.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Movie description cannot exceed {MaxDescriptionLength} characters.");
        }

        return normalizedDescription;
    }
}

