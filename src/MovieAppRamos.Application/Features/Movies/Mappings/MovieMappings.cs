using MovieAppRamos.Application.Features.Movies.DTOs;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Features.Movies.Mappings;

public static class MovieMappings
{
    public static MovieDto ToDto(this Movie movie)
    {
        return new MovieDto(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.ReleaseDate,
            movie.IsDisabled,
            movie.CreatedAt);
    }

    public static MovieDetailsDto ToDetailsDto(this Movie movie)
    {
        var reviewsCount = movie.Reviews.Count;
        decimal? averageRating = reviewsCount == 0
            ? null
            : Math.Round((decimal)movie.Reviews.Average(x => x.Rating), 2, MidpointRounding.AwayFromZero);

        return new MovieDetailsDto(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.ReleaseDate,
            movie.IsDisabled,
            movie.CreatedAt,
            averageRating,
            reviewsCount);
    }
}

