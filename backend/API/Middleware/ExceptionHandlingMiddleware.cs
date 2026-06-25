using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Application.Exceptions;

namespace TP_PROYECTO_SOFTWARE.API.Middleware
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
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (ForbiddenAccessException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                await HandleExceptionAsync(context, StatusCodes.Status409Conflict, "El recurso fue modificado por otra operación. Intente nuevamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado al procesar la solicitud.");
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Ocurrió un error interno. Intente nuevamente más tarde.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = JsonSerializer.Serialize(new { message });
            await context.Response.WriteAsync(response);
        }
    }
}
