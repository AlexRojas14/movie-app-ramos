using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Infrastructure.Persistence.Seeding;

public sealed class DevelopmentDataSeeder
{
    private const int TargetMovies = 300;
    private const int TargetReviews = 1000;
    private const int TargetDisabledMovies = 45;

    private static readonly string[] Adjectives =
    [
        "Lost", "Hidden", "Last", "Golden", "Silent", "Burning", "Infinite", "Broken", "Electric", "Dark",
        "Wild", "Crimson", "Secret", "Neon", "Ancient", "Brave", "Frozen", "Fallen", "Velvet", "Silver"
    ];

    private static readonly string[] Nouns =
    [
        "Horizon", "Empire", "River", "Echo", "Storm", "Signal", "Journey", "Memory", "Shadow", "Labyrinth",
        "Galaxy", "Promise", "Legend", "Reckoning", "Mirage", "Frontier", "Orbit", "Harvest", "Protocol", "Whisper"
    ];

    private static readonly string[] AuthorFirstNames =
    [
        "Alex", "Sofia", "Daniel", "Valeria", "Mateo", "Lucia", "Samuel", "Elena", "Diego", "Carla",
        "Marco", "Paula", "Hector", "Laura", "Nicolas", "Irene", "Javier", "Sara", "Andres", "Diana"
    ];

    private static readonly string[] AuthorLastNames =
    [
        "Ramos", "Lopez", "Garcia", "Mendez", "Castro", "Vega", "Ortega", "Silva", "Romero", "Cortes",
        "Navarro", "Paredes", "Soto", "Fuentes", "Rojas", "Torres", "Salazar", "Molina", "Reyes", "Campos"
    ];

    private static readonly string[] CommentFragments =
    [
        "Excellent pacing and visuals.",
        "Great cast with memorable scenes.",
        "A bit long, but worth watching.",
        "Strong soundtrack and atmosphere.",
        "The ending was unexpected.",
        "Solid direction and production value.",
        "Characters felt realistic.",
        "I would watch it again.",
        "Interesting concept, mixed execution.",
        "One of the best this year."
    ];

    private readonly AppDbContext _dbContext;
    private readonly ILogger<DevelopmentDataSeeder> _logger;

    public DevelopmentDataSeeder(AppDbContext dbContext, ILogger<DevelopmentDataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await EnsureDatabaseReadyAsync(cancellationToken);

        var hasMovies = await _dbContext.Movies.AnyAsync(cancellationToken);
        if (hasMovies)
        {
            _logger.LogInformation("Seeding skipped because data already exists.");
            return;
        }

        var random = new Random(20260215);
        var movies = CreateMovies(random);

        await _dbContext.Movies.AddRangeAsync(movies, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        AddReviews(movies, random);
        DisableSubset(movies, random);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Development seed completed. Movies: {MovieCount}, Reviews: {ReviewCount}, DisabledMovies: {DisabledCount}",
            movies.Count,
            movies.Sum(movie => movie.Reviews.Count),
            movies.Count(movie => movie.IsDisabled));
    }

    private async Task EnsureDatabaseReadyAsync(CancellationToken cancellationToken)
    {
        var hasMigrations = _dbContext.Database.GetMigrations().Any();
        if (hasMigrations)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            await _dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    private static List<Movie> CreateMovies(Random random)
    {
        var movies = new List<Movie>(TargetMovies);

        for (var i = 1; i <= TargetMovies; i++)
        {
            var title = $"{Adjectives[random.Next(Adjectives.Length)]} {Nouns[random.Next(Nouns.Length)]} {i:000}";
            var description = random.NextDouble() < 0.15
                ? null
                : $"A {Adjectives[random.Next(Adjectives.Length)].ToLowerInvariant()} story about {Nouns[random.Next(Nouns.Length)].ToLowerInvariant()}.";
            DateOnly? releaseDate = random.NextDouble() < 0.10
                ? null
                : RandomDate(random);

            movies.Add(Movie.Create(title, description, releaseDate));
        }

        return movies;
    }

    private static void AddReviews(List<Movie> movies, Random random)
    {
        for (var i = 0; i < TargetReviews; i++)
        {
            var movie = movies[random.Next(movies.Count)];
            var authorName = $"{AuthorFirstNames[random.Next(AuthorFirstNames.Length)]} {AuthorLastNames[random.Next(AuthorLastNames.Length)]}";
            var rating = random.Next(Review.MinRating, Review.MaxRating + 1);
            var comment = random.NextDouble() < 0.20
                ? null
                : CommentFragments[random.Next(CommentFragments.Length)];

            movie.AddReview(authorName, rating, comment);
        }
    }

    private static void DisableSubset(List<Movie> movies, Random random)
    {
        foreach (var movie in movies.OrderBy(_ => random.Next()).Take(TargetDisabledMovies))
        {
            movie.Disable();
        }
    }

    private static DateOnly RandomDate(Random random)
    {
        var year = random.Next(1970, DateTime.UtcNow.Year + 1);
        var month = random.Next(1, 13);
        var day = random.Next(1, 29);
        return new DateOnly(year, month, day);
    }
}
