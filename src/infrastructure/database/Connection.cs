using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using config;
public class DatabaseConnection
{
    private readonly ILogger<DatabaseConnection> _logger;
    private readonly SqlConnection _connection;

    public DatabaseConnection(
        IOptions<DatabaseConfig> dbConfig,
        ILogger<DatabaseConnection> logger
    )
    {
        _logger = logger;
        try
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dbConfig.Value.DataSource,
                UserID = dbConfig.Value.UserID,
                Password = dbConfig.Value.Password,
                InitialCatalog = dbConfig.Value.InitialCatalog,
                Encrypt = dbConfig.Value.Encrypt
            };
            _connection = new SqlConnection(builder.ConnectionString);
            _connection.Open();
        }
        catch (SqlException e)
        {
            _logger.LogError($"Error on connect Sql Server {e.ToString()}");
        }
    }
    public async Task<SqlConnection> GetConnection()
    {
        return _connection;
    }
}
