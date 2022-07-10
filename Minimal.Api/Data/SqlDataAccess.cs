using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Minimal.Api.Data
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly IConfiguration _configuration;

        public SqlDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> LoadAllData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            var result = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
