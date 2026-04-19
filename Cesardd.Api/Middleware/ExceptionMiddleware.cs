using Cesardd.Shared.Exceptions;
using Cesardd.Shared.Results;
using System.Net;

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
                var response = ApiResponse<object>.Fail(ex.Message);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = ApiResponse<object>.Fail("Error interno en el servidor");

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
