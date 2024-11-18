using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Features.OrderCloud.Commands.DeleteUsers;
using XC.OC.Migration.Core.Application.Features.OrderCloud.Commands.DuplicateUsers;
using XC.OC.Migration.Core.Application.Features.Users.Queries.GetUsersList;

namespace XC.OC.Migration.Core.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
            services.AddScoped<IDeleteMigratedUsersCommand, DeleteMigratedUsersCommand>();
            services.AddScoped<IFindDuplicateUsersCommand, FindDuplicateUsersCommand>();
            services.AddScoped<IGetUsersListQuery, GetUsersListQuery>();
            return services;
        }
    }
}
