using System.Data;
using Dapper;

namespace SmugglerSelectWeb.Services.DB;

public class UserRepository
{
    private readonly IDbConnection _db;

    public UserRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        string sql = "SELECT id, name, email FROM users";

        return await _db.QueryAsync<User>(sql);
    }

    public async Task<User?> GetUser(int id)
    {
        string sql = "SELECT id, name, email FROM users WHERE id=@Id";

        return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<int> InsertUser(User user)
    {
        string sql = @"
        INSERT INTO users(name,email)
        VALUES(@Name,@Email)
        RETURNING id";

        return await _db.ExecuteScalarAsync<int>(sql, user);
    }
}

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public int isuse { get; set; }
}