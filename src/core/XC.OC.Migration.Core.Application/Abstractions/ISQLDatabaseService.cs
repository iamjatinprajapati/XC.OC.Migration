using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Domain.Model.ExperienceCommerce;

namespace XC.OC.Migration.Core.Application.Abstractions
{
    public interface ISQLDatabaseService
    {
        IDbConnection GetConnection(string connectionStringName, bool openConnection = false);

        IDbDataParameter CreateParameter(string name, object value, 
            System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input);

        IDbCommand CreateCommand(IDbConnection connection, string query, 
            System.Data.CommandType commandType = System.Data.CommandType.Text, params IDbDataParameter[]? parameters);

        Task<long> GetUsersCount(string userNamePrefix, string startDate, string endDate, int pageIndex,
            int pageSize, string applicationName);

        Task<IEnumerable<User>> GetUsers(string userNamePrefix, string startDate, string endDate, int pageIndex,
            int pageSize, string applicationName);
    }
}
