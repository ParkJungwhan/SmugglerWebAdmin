using Dapper;
using Microsoft.AspNetCore.Identity;
using SmugglerWebCommon.Data;

namespace SmugglerWebTool.Data;

public sealed class DapperUserStore(IDbConnectionFactory connectionFactory)
    : IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
{
    public void Dispose()
    {
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        user.Id = string.IsNullOrWhiteSpace(user.Id) ? Guid.NewGuid().ToString("N") : user.Id;

        const string sql = """
            INSERT INTO users (user_id, user_name, user_email, user_ps, admintype)
            VALUES (@UserId, @UserName, @UserEmail, @UserPs, @AdminType);
            """;

        using var connection = connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            UserId = user.Id,
            UserName = user.UserName,
            UserEmail = user.Email,
            UserPs = user.PasswordHash,
            AdminType = user.AdminType
        });

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        const string sql = """
            UPDATE users
            SET
                user_name = @UserName,
                user_email = @UserEmail,
                user_ps = @UserPs,
                admintype = @AdminType
            WHERE user_id = @UserId;
            """;

        using var connection = connectionFactory.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new
        {
            UserId = user.Id,
            UserName = user.UserName,
            UserEmail = user.Email,
            UserPs = user.PasswordHash,
            AdminType = user.AdminType
        });

        return affected == 1
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError { Description = "User update failed." });
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        const string sql = "DELETE FROM users WHERE user_id = @UserId;";

        using var connection = connectionFactory.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new { UserId = user.Id });

        return affected == 1
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError { Description = "User delete failed." });
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
            SELECT
                user_id AS Id,
                user_name AS UserName,
                UPPER(user_name) AS NormalizedUserName,
                user_email AS Email,
                UPPER(user_email) AS NormalizedEmail,
                TRUE AS EmailConfirmed,
                user_ps AS PasswordHash,
                admintype AS AdminType
            FROM users
            WHERE user_id = @UserId;
            """;

        using var connection = connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { UserId = userId });
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
            SELECT
                user_id AS Id,
                user_name AS UserName,
                UPPER(user_name) AS NormalizedUserName,
                user_email AS Email,
                UPPER(user_email) AS NormalizedEmail,
                TRUE AS EmailConfirmed,
                user_ps AS PasswordHash,
                admintype AS AdminType
            FROM users
            WHERE UPPER(user_name) = @NormalizedUserName;
            """;

        using var connection = connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { NormalizedUserName = normalizedUserName });
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.Id ?? string.Empty);
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(true);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
            SELECT
                user_id AS Id,
                user_name AS UserName,
                UPPER(user_name) AS NormalizedUserName,
                user_email AS Email,
                UPPER(user_email) AS NormalizedEmail,
                TRUE AS EmailConfirmed,
                user_ps AS PasswordHash,
                admintype AS AdminType
            FROM users
            WHERE UPPER(user_email) = @NormalizedEmail;
            """;

        using var connection = connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { NormalizedEmail = normalizedEmail });
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }
}

