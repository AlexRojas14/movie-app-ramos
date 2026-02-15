using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieAppRamos.Application.Common.Exceptions;
using MovieAppRamos.Domain.Exceptions;

namespace MovieAppRamos.Api.Common;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        switch (exception)
        {
            case ValidationException validationException:
                problemDetails.Title = "Validation failed.";
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Type = "https://httpstatuses.com/422";
                problemDetails.Detail = "One or more validation errors occurred.";
                problemDetails.Extensions["errors"] = validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).ToArray());

                _logger.LogWarning(
                    exception,
                    "Validation failure on {Path}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    httpContext.TraceIdentifier);
                break;

            case NotFoundException:
                problemDetails.Title = "Resource not found.";
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Type = "https://httpstatuses.com/404";
                problemDetails.Detail = exception.Message;
                _logger.LogInformation(
                    "NotFound on {Path}. Message: {Message}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    exception.Message,
                    httpContext.TraceIdentifier);
                break;

            case DomainException:
                problemDetails.Title = "Business rule conflict.";
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Type = "https://httpstatuses.com/409";
                problemDetails.Detail = exception.Message;
                _logger.LogWarning(
                    exception,
                    "Business conflict on {Path}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    httpContext.TraceIdentifier);
                break;

            case ArgumentException:
            case FormatException:
                problemDetails.Title = "Bad request.";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = "https://httpstatuses.com/400";
                problemDetails.Detail = exception.Message;
                _logger.LogWarning(
                    exception,
                    "BadRequest on {Path}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    httpContext.TraceIdentifier);
                break;

            default:
                problemDetails.Title = "Internal server error.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Type = "https://httpstatuses.com/500";
                problemDetails.Detail = "An unexpected error occurred.";
                _logger.LogError(
                    exception,
                    "Unhandled exception on {Path}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    httpContext.TraceIdentifier);
                break;
        }

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
