using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Models;
using XC.OC.Migration.Core.Domain.Model.ExperienceCommerce;
using XC.OC.Migration.Core.Domain.Model.OrderCloud;

namespace XC.OC.Migration.Core.Application.Abstractions
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Domain.Model.ExperienceCommerce.User>> GetUsers(string userNamePrefix, string startDate, string endDate, int pageIndex = 1, int pageSize = 50,
            string applicationName = "sitecore");

        Task<long> GetUsersCount(string userNamePrefix, string startDate, string endDate, int pageIndex = 1, int pageSize = 50,
            string applicationName = "sitecore");

        Task<PagedResults<Domain.Model.OrderCloud.User>> GetOrderCloudUsers();

        Task<CustomerEntity?> GetXCEntity(string customerId);

        Task DeleteOrderCloudMigratedUsers(int iterations);
    }
}
