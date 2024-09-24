using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Application.Models;
using XC.OC.Migration.Core.Domain.Model.ExperienceCommerce;
using XC.OC.Migration.Core.Domain.Model.OrderCloud;
using XC.OC.Migration.Infrastructure.Persistence.Services;
using User = XC.OC.Migration.Core.Domain.Model.OrderCloud.User;

namespace XC.OC.Migration.Infrastructure.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IOrderCloudDataService _orderCloudDataService;
        private readonly ISQLDatabaseService _sqlDatabaseService;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(IOrderCloudDataService orderCloudDataService, ILogger<UsersRepository> logger,
            ISQLDatabaseService sqlDatabaseService) { 
            _orderCloudDataService = orderCloudDataService;
            _logger = logger;
            _sqlDatabaseService = sqlDatabaseService;
        }

        public async Task<PagedResults<User>> GetOrderCloudUsers()
        {
            Models.TokenResponse authToken = await _orderCloudDataService.AuthenticateUsingClientCredentialsAsync();
            if (authToken == null)
            {
                throw new UnauthorizedAccessException();
            }
            var data = await _orderCloudDataService.ListUsers(authToken.AccessToken);
            return data;
        }

        public async Task<long> GetUsersCount(string userNamePrefix, string startDate, string endDate, int pageIndex = 1, int pageSize = 50, string applicationName = "sitecore")
        {
            return await _sqlDatabaseService.GetUsersCount(userNamePrefix, startDate, endDate, pageIndex, pageSize, applicationName);
        }

        public async Task<IEnumerable<Core.Domain.Model.ExperienceCommerce.User>> GetUsers(string userNamePrefix, string startDate, string endDate, int pageIndex, int pageSize, string applicationName)
        {
            return await _sqlDatabaseService.GetUsers(userNamePrefix, startDate, endDate, pageIndex, pageSize, applicationName);
        }

        public async Task DeleteOrderCloudMigratedUsers(OrderCloudDeleteMigratedUsersRequest request)
        {
            Models.TokenResponse authToken = await _orderCloudDataService.AuthenticateUsingClientCredentialsAsync();
            if(authToken == null)
            {
                throw new UnauthorizedAccessException();
            }
            var data = await _orderCloudDataService.ListUsers(authToken.AccessToken);
        }
    }
}
