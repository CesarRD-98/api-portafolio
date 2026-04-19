using Cesardd.Api;
using Cesardd.Api.Middleware;
using Cesardd.Core;
using Cesardd.Infrastructure;
using Cesardd.Shared.Results;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services
    .AddApi()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://www.cesardd.com",
                "http://localhost:3000",
                "https://api.cesardd.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        var response = ApiResponse<object>.Fail("Demasiadas solicitudes, intenta más tarde");

        await context.HttpContext.Response.WriteAsJsonAsync(response, token);
    };

    options.AddFixedWindowLimiter("ContactPolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
});

var app = builder.Build();

// Dev tools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware pipeline
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("FrontendPolicy");

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();