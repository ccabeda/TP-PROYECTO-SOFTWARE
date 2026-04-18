using System.Text.Json;

namespace TP_PROYECTO_SOFTWARE.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
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
