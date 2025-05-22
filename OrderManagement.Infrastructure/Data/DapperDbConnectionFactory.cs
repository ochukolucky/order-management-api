using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OrderManagement.Infrastructure.Data
{
    public class DapperDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperDbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? "Server=(localdb)\\MSSQLLocalDB;Database=OrderDb;Trusted_Connection=True;";
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
