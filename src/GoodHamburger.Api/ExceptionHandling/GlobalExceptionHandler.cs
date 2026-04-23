using FluentValidation;
using GoodHamburger.Application.Exceptions;
using GoodHamburger.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is DomainException or NotFoundException or ValidationException)
        {
            logger.LogWarning(exception, "{Message}", exception.Message);
        }
        else
        {
            logger.LogError(exception, "Falha interna.");
        }

        var (status, problem) = Map(exception, httpContext.TraceIdentifier);
        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    private static (int Status, ProblemDetails Problem) Map(Exception exception, string traceId)
    {
        return exception switch
        {
            ValidationException vex => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Title = "Dados inválidos",
                    Detail = "Revise os campos informados.",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Instance = traceId,
                    Extensions =
                    {
                        ["errorCode"] = "validation_error",
                        ["errors"] = vex.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray())
                    }
                }),
            DomainException dex => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Title = "Regra de negócio violada",
                    Detail = dex.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Instance = traceId,
                    Extensions = { ["errorCode"] = dex.ErrorCode }
                }),
            NotFoundException nf => (
                StatusCodes.Status404NotFound,
                new ProblemDetails
                {
                    Title = "Recurso não encontrado",
                    Detail = nf.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Instance = traceId,
                    Extensions = { ["errorCode"] = nf.ErrorCode }
                }),
            _ => (
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Erro interno",
                    Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Instance = traceId,
                    Extensions = { ["errorCode"] = "internal_error" }
                })
        };
    }
}
