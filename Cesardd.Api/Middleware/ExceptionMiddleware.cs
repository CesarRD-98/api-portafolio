using Cesardd.Shared.Exceptions;
using Cesardd.Shared.Results;
using System.Net;

namespace Cesardd.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Error controlado al procesar {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await WriteErrorResponse(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado al procesar {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await WriteErrorResponse(context, (int)HttpStatusCode.InternalServerError,
                    "Error interno en el servidor");
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(message));
        }
    }
}
