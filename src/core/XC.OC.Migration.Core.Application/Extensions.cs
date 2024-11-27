using Microsoft.Extensions.DependencyInjection;
using XC.OC.Migration.Core.Application.Features.Users.Commands.ExportUsersCommand;

namespace XC.OC.Migration.Core.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
            services.AddScoped<IExportUsersCommand, ExportUsersCommand>();
            return services;
        }
    }
}
