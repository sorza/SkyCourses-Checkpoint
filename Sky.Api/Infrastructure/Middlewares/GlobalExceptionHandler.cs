using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Sky.Api.Infrastructure.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
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
            _logger.LogError(exception, "Exceção capturada: {Message}", exception.Message);

            var (statusCode, title, detail) = exception switch
            {
                ArgumentException argEx => (
                    StatusCodes.Status400BadRequest,
                    "Validação falhou",
                    argEx.Message
                ),
                InvalidOperationException invalidOpEx => (
                    StatusCodes.Status400BadRequest,
                    "Operação inválida",
                    invalidOpEx.Message
                ),
                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    "Recurso não encontrado",
                    exception.Message
                ),
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Não autorizado",
                    "Você não tem permissão para acessar este recurso."
                ),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor",
                    "Ocorreu um erro inesperado. Tente novamente mais tarde."
                )
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; 
        }
    }
}