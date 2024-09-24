using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Infrastructure.Persistence.Repositories;
using XC.OC.Migration.Infrastructure.Persistence.Services;
using XC.OC.Migration.Infrastructure;
using Microsoft.Extensions.Configuration;
using XC.OC.Migration.Core.Domain.Model.Configurations;

namespace XC.OC.Migration.Infrastructure.Persistence
{
    public static class Extensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddInfrastructureServices(configuration);
            services.Configure<OrderCloudSettings>(configuration.GetSection(OrderCloudSettings.OrderCloudSettingsName));
            services.AddScoped<ISQLDatabaseService, SQLDatabaseService>();
            services.AddScoped<IOrderCloudDataService, OrderCloudDataService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            return services;
        }
    }
}
