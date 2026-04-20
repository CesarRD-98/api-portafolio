namespace Cesardd.Api.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration config, ILogger<ApiKeyMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _config = config;
        private readonly ILogger<ApiKeyMiddleware> _logger = logger;

        private const string HEADER = "x-api-key";

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/openapi"))
            {
                await _next(context);
                return;
            }

            var apiKey = context.Request.Headers[HEADER].FirstOrDefault();
            var validKey = _config["Security:ApiKey"];

            if (string.IsNullOrEmpty(apiKey) || apiKey != validKey)
            {
                _logger.LogWarning("Acceso no autorizado desde {IP}", context.Connection.RemoteIpAddress);
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    error = new { message = "Unauthorized" }
                });

                return;
            }

            await _next(context);

        }
    }
}
