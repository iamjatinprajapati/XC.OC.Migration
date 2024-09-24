using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Domain.Model.ExperienceCommerce;

namespace XC.OC.Migration.Core.Application.Features.Users.Queries.GetUsersList
{
    public interface IGetUsersListQuery
    {
        Task<IEnumerable<User>> Execute(string userNamePrefix, string startDate, string endDate, int pageIndex = 1, int pageSize = 300
            ,string applicationName = "sitecore");
    }
}
