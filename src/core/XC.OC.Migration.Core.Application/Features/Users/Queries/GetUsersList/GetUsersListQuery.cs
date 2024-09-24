using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Domain.Model.ExperienceCommerce;

namespace XC.OC.Migration.Core.Application.Features.Users.Queries.GetUsersList
{
    public class GetUsersListQuery(IUsersRepository usersRepository, ILogger<GetUsersListQuery> logger) : IGetUsersListQuery
    {
        public async Task<IEnumerable<User>> Execute(string userNamePrefix, string startDate, string endDate, int pageIndex = 1, int pageSize = 300, string applicationName = "sitecore")
        {
            if(logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Executing get experience commerce users query: {userNamePrefix}, {startDate}, {endDate}, {pageIndex}, {pageSize}, {applicationName}",
                    userNamePrefix, startDate, endDate, pageIndex, pageSize, applicationName);

            return await usersRepository.GetUsers(userNamePrefix, startDate, endDate, pageIndex, pageSize, applicationName);
        }
    }
}
