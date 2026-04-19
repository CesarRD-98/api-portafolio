using Cesardd.Core.Interfaces;
using Cesardd.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace Cesardd.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(opt =>
            {
                opt.ApiToken = configuration["Resend:ApiKey"]!;
            });
            services.Configure<ResendOptions>(configuration.GetSection("Resend"));
            services.AddTransient<IResend, ResendClient>();
            services.AddScoped<IEmailService, ResendEmailService>();

            return services;
        }
    }
}
