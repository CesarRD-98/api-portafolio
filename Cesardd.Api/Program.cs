using Cesardd.Api;
using Cesardd.Api.Extensions;
using Cesardd.Core;
using Cesardd.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services
    .AddApi()
    .AddCore()
    .AddInfrastructure(builder.Configuration)
    .AddApiPolicies();

var app = builder.Build();

// Dev tools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseApiMiddlewares();

app.MapGroup("/sv").MapControllers();

app.Run();