using Cesardd.Shared.Exceptions;
using Cesardd.Shared.Results;
using System.Net;
using System.Text.Json;

namespace Cesardd.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Fail(ex.Message);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Fail("Error interno en el servidor");

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
