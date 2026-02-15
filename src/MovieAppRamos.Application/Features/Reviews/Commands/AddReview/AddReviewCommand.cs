using MediatR;
using MovieAppRamos.Application.Features.Reviews.DTOs;

namespace MovieAppRamos.Application.Features.Reviews.Commands.AddReview;

public sealed record AddReviewCommand(
    Guid MovieId,
    string AuthorName,
    int Rating,
    string? Comment) : IRequest<ReviewDto>;

