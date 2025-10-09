using Npgsql;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace skillsphere.infrastructure.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            // Get connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException("DefaultConnection not found in configuration");
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
