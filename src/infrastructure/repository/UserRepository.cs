using Microsoft.Data.SqlClient;
using System.Data;
using core.entity;
using core.interfaces;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly DatabaseConnection _database;

    public UserRepository(
        ILogger<UserRepository> logger,
        DatabaseConnection database
    )
    {
        _logger = logger;
        _database = database;
    }
    public async Task<List<User>> CreateAndListUsersAsync(User u)
    {
        var connection = await _database.GetConnection();
        try
        {
            var users = new List<User>();
            using (var command = new SqlCommand("InsertAndListAllUsers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", u.Name);
                command.Parameters.AddWithValue("@Age", u.Age);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userName = reader.GetString(0);
                        var userAge = reader.GetInt32(1);
                        users.Add(new User
                        {
                            Age = userAge,
                            Name = userName
                        });
                    }
                }
            }
            _logger.LogInformation($"User founded and returned");
            return users;
        }
        catch (SqlException e)
        {
            _logger.LogError($"Error on execute Sql Server Procedure {e.ToString()}");
            throw e;
        }

    }
}
