using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Infrastructure.Services;

namespace XC.OC.Migration.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IFlurlClientCache>(sp => new FlurlClientCache());
            services.AddSingleton<IMessageQueueService, MessageQueueService>();
            services.AddScoped<IRESTService, RestService>();
            return services;
        }
    }
}
