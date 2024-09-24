using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Features.Users.Queries.GetUsersList;

namespace XC.OC.Migration.Core.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
            services.AddScoped<IGetUsersListQuery, GetUsersListQuery>();
            return services;
        }
    }
}
