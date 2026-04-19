using Cesardd.Core.Features.Contact.SendContact;
using Microsoft.Extensions.DependencyInjection;

namespace Cesardd.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<SendContactHandler>();

            return services;
        }
    }
}
