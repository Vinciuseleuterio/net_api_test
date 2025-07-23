using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Presentation.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado");

                var problemDetails = CreateProblemDetails(context, ex);

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

        private ProblemDetails CreateProblemDetails(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                DbUpdateException => StatusCodes.Status500InternalServerError,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return new ProblemDetails
            {
                Type = statusCode switch
                {
                    400 => "https://httpstatuses.com/400",
                    404 => "https://httpstatuses.com/404",
                    500 => "https://httpstatuses.com/500",
                    _ => "https://httpstatuses.com/500"
                },
                Title = statusCode switch
                {
                    400 => "Erro de validação",
                    404 => "Recurso não encontrado",
                    500 => "Erro interno do servidor",
                    _ => "Erro desconhecido"
                },
                Status = statusCode,
                Detail = ex.Message,          // Detalhe do erro
                Instance = context.Request.Path // Caminho da request
            };
        }
    }
}

