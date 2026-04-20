using Cesardd.Api.Middleware;

namespace Cesardd.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors("FrontendPolicy");

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseRateLimiter();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            return app;
        }
    }
}
