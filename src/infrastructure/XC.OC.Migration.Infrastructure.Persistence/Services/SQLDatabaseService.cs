using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Domain.Model.ExperienceCommerce;

namespace XC.OC.Migration.Infrastructure.Persistence.Services
{
    public class SQLDatabaseService(IConfiguration configuration, ILogger<SQLDatabaseService> logger) : ISQLDatabaseService
    {
        public IDbCommand CreateCommand(IDbConnection connection, string query, 
            System.Data.CommandType commandType = System.Data.CommandType.Text, params IDbDataParameter[]? parameters)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Creating command");
            IDbCommand command = connection.CreateCommand();
            //command.Connection = connection;
            command.CommandText = query;
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (IDbDataParameter parameter in parameters) { command.Parameters.Add(parameter); }
            }
            return command;
        }

        public IDbDataParameter CreateParameter(string name, object value,
            System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Creating parameter");
            var parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = direction;
            return parameter;
        }

        public IDbConnection GetConnection(string connectionStringName, bool openConnection = false)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Creating connection");
            var connection = new SqlConnection(configuration.GetConnectionString(connectionStringName));
            if (openConnection)
            {
                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug("Opening connection");
                connection.Open();
            }
            return connection;
        }

        public Task<IEnumerable<User>> GetUsers(string userNamePrefix, string startDate, string endDate, int pageIndex, int pageSize, string applicationName)
        {
            return Task.Run(() =>
            {
                var users = new List<User>();
                using(IDbConnection connection = GetConnection("CoreDatabase"))
                {
                    using(IDbCommand command = CreateCommand(connection, "dbo.Migration_aspnet_Membership_GetAllUsers",
                        CommandType.StoredProcedure, new SqlParameter("@ApplicationName", applicationName),
                        new SqlParameter("@UserNamePrefix", userNamePrefix),
                        new SqlParameter("@StartDate", startDate),
                        new SqlParameter("@EndDate", endDate),
                        new SqlParameter("@PageIndex", pageIndex),
                        new SqlParameter("@PageSize", pageSize),
                        new SqlParameter
                        {
                            ParameterName = "@TotalRecords",
                            DbType = DbType.Int64,
                            Direction = System.Data.ParameterDirection.ReturnValue,
                        }))
                    {
                        connection.Open();
                        IDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            var record = (IDataRecord)reader;
                            var user = new User()
                            {
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                                LastLoginDate = reader.GetDateTime(reader.GetOrdinal("LastLoginDate")),
                                LastLockoutDate = reader.GetDateTime(reader.GetOrdinal("LastLockoutDate")),
                                IsLockedOut = reader.GetBoolean(reader.GetOrdinal("IsLockedOut")),
                                PropertyNames = reader.GetString(reader.GetOrdinal("PropertyNames")).Split(":"),
                                PropertyValuesString = reader.GetString(reader.GetOrdinal("PropertyValuesString")),
                                PropertyValuesBinary = (byte[])reader["PropertyValuesBinary"]
                            };
                            ExtractUserProfileProperties(user);
                            users.Add(user);
                        }
                    }
                }
                return users.AsEnumerable();
            });
        }

        private void ExtractUserProfileProperties(User user)
        {
            try
            {
                for(int index = 0; index < user.PropertyNames.Length / 4; ++index)
                {
                    string name = user.PropertyNames[index * 4];
                    int num = int.Parse(user.PropertyNames[index * 4 + 2], CultureInfo.InvariantCulture);
                    int length = int.Parse(user.PropertyNames[index * 4 + 3], CultureInfo.InvariantCulture);
                    if (user.PropertyNames[index * 4 + 1] == "S" && num >= 0 && length > 0 && user.PropertyValuesString.Length >= num + length)
                    {
                        switch(name.ToLower())
                        {
                            case "fullname":
                                user.FullName = user.PropertyValuesString.Substring(num, length);
                                break;
                        }
                    }

                    if (user.PropertyNames[index * 4 + 1] == "B" && num >= 0 && length > 0 &&
                        user.PropertyValuesBinary.Length >= num + length)
                    {
                        byte[] destintation = new byte[length];
                        Buffer.BlockCopy(user.PropertyValuesBinary, num, destintation, 0, length);
                        var serialized = (object)destintation;
                        if(!(serialized is string))
                        {
                            using(var ms = new MemoryStream((byte[])serialized))
                            {
                                try
                                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                                    Dictionary<string, string>? props = (Dictionary<string, string>)new BinaryFormatter().Deserialize(ms);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                                    logger.LogInformation(JsonSerializer.Serialize(props));
                                    foreach(var key in props.Keys)
                                    {
                                        if(!key.Equals("__serialization", StringComparison.OrdinalIgnoreCase))
                                        {
                                            logger.LogInformation("Serialized prop name: {key}", key);
                                            switch(key.ToLower())
                                            {
                                                case "city":
                                                    user.City = props[key];
                                                    break;
                                                case "country":
                                                    user.Country = props[key];
                                                    break;
                                                case "stateprovince":
                                                    user.StateProvince = props[key];
                                                    break;
                                                //case "firstname":
                                                //    user.FirstName = props[key];
                                                //    break;
                                                //case "lastname":
                                                //    user.LastName = props[key];
                                                //    break;
                                                case "address1":
                                                    user.Address1 = props[key];
                                                    break;
                                                case "address2":
                                                    user.Address2 = props[key];
                                                    break;
                                            }
                                            continue;
                                        }
                                        user.ExtendedProperties = JsonSerializer.Deserialize<ExtendedProperties>(props[key]);                                        
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex.Message, ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<long> GetUsersCount(string userNamePrefix, string startDate, string endDate, int pageIndex,
            int pageSize, string applicationName)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = GetConnection("CoreDatabase"))
                {
                    using (IDbCommand command = CreateCommand(connection, "dbo.Migration_aspnet_Membership_CountUsers",
                        CommandType.StoredProcedure, new SqlParameter("@ApplicationName", applicationName),
                        new SqlParameter("@UserNamePrefix", userNamePrefix),
                        new SqlParameter("@StartDate", startDate),
                        new SqlParameter("@EndDate", endDate),
                        new SqlParameter
                        {
                            ParameterName = "@TotalRecords",
                            DbType = DbType.Int64,
                            Direction = System.Data.ParameterDirection.ReturnValue,
                        }))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        var value = ((SqlParameter)command.Parameters["@TotalRecords"]).Value;
                        var totalRecords = long.Parse(value.ToString());
                        return totalRecords;
                    }
                }
            });
        }
    }
}
