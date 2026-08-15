using System.Text.Json;

namespace Cesardd.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var message = context.ModelState.Values
                        .SelectMany(value => value.Errors)
                        .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                            ? "La solicitud no es válida"
                            : error.ErrorMessage)
                        .FirstOrDefault() ?? "La solicitud no es válida";

                    return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(
                        Cesardd.Shared.Results.ApiResponse<object>.Fail(message));
                };
            });

            services.AddOpenApi();

            return services;
        }
    }
}
