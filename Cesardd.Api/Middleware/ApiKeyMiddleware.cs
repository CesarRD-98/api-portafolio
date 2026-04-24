namespace Cesardd.Api.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration config, ILogger<ApiKeyMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _config = config;
        private readonly ILogger<ApiKeyMiddleware> _logger = logger;

        private const string HEADER = "x-api-key";

        private static readonly string[] PublicPaths = ["/sv/health", "/openapi"];

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            if (PublicPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var apiKey = context.Request.Headers[HEADER].FirstOrDefault();
            var validKey = _config["Security:ApiKey"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(validKey) || !SecureEquals(apiKey, validKey))
            {
                _logger.LogWarning("Acceso no autorizado desde {IP}", context.Connection.RemoteIpAddress);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    error = new { message = "Unauthorized" }
                });

                return;
            }

            await _next(context);
        }

        private static bool SecureEquals(string a, string b)
        {
            if (a.Length != b.Length) return false;

            var result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}