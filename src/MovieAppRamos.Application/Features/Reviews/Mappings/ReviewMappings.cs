using MovieAppRamos.Application.Features.Reviews.DTOs;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Application.Features.Reviews.Mappings;

public static class ReviewMappings
{
    public static ReviewDto ToDto(this Review review)
    {
        return new ReviewDto(
            review.Id,
            review.MovieId,
            review.AuthorName,
            review.Rating,
            review.Comment,
            review.CreatedAt);
    }
}

