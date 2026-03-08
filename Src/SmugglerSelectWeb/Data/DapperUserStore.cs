using Dapper;
using Microsoft.AspNetCore.Identity;
using SmugglerWebCommon.Data;

namespace SmugglerSelectWeb.Data;

public sealed class DapperUserStore(IDbConnectionFactory connectionFactory)
    : IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>
{
    public void Dispose()
    {
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        user.Id = string.IsNullOrWhiteSpace(user.Id) ? Guid.NewGuid().ToString("N") : user.Id;
        user.SecurityStamp = string.IsNullOrWhiteSpace(user.SecurityStamp) ? Guid.NewGuid().ToString("N") : user.SecurityStamp;

        const string sql = """
            INSERT INTO users (
                id,
                user_name,
                normalized_user_name,
                email,
                normalized_email,
                email_confirmed,
                password_hash,
                security_stamp
            ) VALUES (
                @Id,
                @UserName,
                @NormalizedUserName,
                @Email,
                @NormalizedEmail,
                @EmailConfirmed,
                @PasswordHash,
                @SecurityStamp
            );
            """;

        using var connection = connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.SecurityStamp
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
                normalized_user_name = @NormalizedUserName,
                email = @Email,
                normalized_email = @NormalizedEmail,
                email_confirmed = @EmailConfirmed,
                password_hash = @PasswordHash,
                security_stamp = @SecurityStamp
            WHERE id = @Id;
            """;

        using var connection = connectionFactory.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.SecurityStamp
        });

        return affected == 1
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError { Description = "User update failed." });
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        const string sql = "DELETE FROM users WHERE id = @Id;";

        using var connection = connectionFactory.CreateConnection();
        var affected = await connection.ExecuteAsync(sql, new { user.Id });

        return affected == 1
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError { Description = "User delete failed." });
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
            SELECT
                id AS Id,
                user_name AS UserName,
                normalized_user_name AS NormalizedUserName,
                email AS Email,
                normalized_email AS NormalizedEmail,
                email_confirmed AS EmailConfirmed,
                password_hash AS PasswordHash,
                security_stamp AS SecurityStamp
            FROM users
            WHERE id = @UserId;
            """;

        using var connection = connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { UserId = userId });
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
            SELECT
                id AS Id,
                user_name AS UserName,
                normalized_user_name AS NormalizedUserName,
                email AS Email,
                normalized_email AS NormalizedEmail,
                email_confirmed AS EmailConfirmed,
                password_hash AS PasswordHash,
                security_stamp AS SecurityStamp
            FROM users
            WHERE normalized_user_name = @NormalizedUserName;
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
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
            SELECT
                id AS Id,
                user_name AS UserName,
                normalized_user_name AS NormalizedUserName,
                email AS Email,
                normalized_email AS NormalizedEmail,
                email_confirmed AS EmailConfirmed,
                password_hash AS PasswordHash,
                security_stamp AS SecurityStamp
            FROM users
            WHERE normalized_email = @NormalizedEmail;
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

    public Task SetSecurityStampAsync(ApplicationUser user, string? stamp, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }

    public Task<string?> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.SecurityStamp);
    }
}

