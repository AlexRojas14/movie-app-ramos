using Moq;
using MovieAppRamos.Application.Abstractions.Persistence;
using MovieAppRamos.Application.Features.Movies.Queries.ListMovies;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Tests.Features.Movies.Queries;

public sealed class ListMoviesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnConsistentPagination()
    {
        var movies = Enumerable.Range(1, 25)
            .Select(index => Movie.Create($"Movie {index:00}"))
            .ToList();

        var repository = BuildRepository(movies);
        var handler = new ListMoviesQueryHandler(repository.Object);

        var result = await handler.Handle(
            new ListMoviesQuery(
                SortBy: MovieSortBy.Title,
                SortDir: SortDirection.Asc,
                Page: 2,
                PageSize: 10),
            CancellationToken.None);

        Assert.Equal(25, result.TotalCount);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal("Movie 11", result.Items[0].Title);
        Assert.Equal("Movie 20", result.Items[^1].Title);
    }

    [Fact]
    public async Task Handle_ShouldSortByTitleAscAndDesc()
    {
        var movies = new List<Movie>
        {
            Movie.Create("Beta"),
            Movie.Create("Alpha"),
            Movie.Create("Charlie")
        };

        var repository = BuildRepository(movies);
        var handler = new ListMoviesQueryHandler(repository.Object);

        var asc = await handler.Handle(
            new ListMoviesQuery(SortBy: MovieSortBy.Title, SortDir: SortDirection.Asc),
            CancellationToken.None);

        var desc = await handler.Handle(
            new ListMoviesQuery(SortBy: MovieSortBy.Title, SortDir: SortDirection.Desc),
            CancellationToken.None);

        Assert.Equal(["Alpha", "Beta", "Charlie"], asc.Items.Select(x => x.Title).ToArray());
        Assert.Equal(["Charlie", "Beta", "Alpha"], desc.Items.Select(x => x.Title).ToArray());
    }

    [Fact]
    public async Task Handle_ShouldFilterBySearch()
    {
        var movies = new List<Movie>
        {
            Movie.Create("Avatar"),
            Movie.Create("The Aviator"),
            Movie.Create("Matrix")
        };

        var repository = BuildRepository(movies);
        var handler = new ListMoviesQueryHandler(repository.Object);

        var result = await handler.Handle(
            new ListMoviesQuery(Search: "av", SortBy: MovieSortBy.Title, SortDir: SortDirection.Asc),
            CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(["Avatar", "The Aviator"], result.Items.Select(x => x.Title).ToArray());
    }

    private static Mock<IMovieRepository> BuildRepository(List<Movie> movies)
    {
        var repository = new Mock<IMovieRepository>();

        repository.Setup(x => x.Query())
            .Returns(movies.AsQueryable());

        return repository;
    }
}

