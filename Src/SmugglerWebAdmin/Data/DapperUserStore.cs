using Dapper;
using Microsoft.AspNetCore.Identity;

namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// Dapper + PostgreSQL 기반 ASP.NET Core Identity 사용자 저장소.<br/>
    /// IUserPasswordStore, IUserEmailStore, IUserSecurityStampStore,
    /// IUserLockoutStore, IUserTwoFactorStore 를 구현합니다.
    /// </summary>
    public class DapperUserStore :
        IUserStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserSecurityStampStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DapperUserStore(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ── IUserStore ────────────────────────────────────────────

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken ct)
        {
            user.Id = Guid.NewGuid().ToString();
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            user.SecurityStamp = Guid.NewGuid().ToString();

            const string sql = """
                INSERT INTO "AspNetUsers"
                    ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
                     "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
                     "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
                     "LockoutEnd", "LockoutEnabled", "AccessFailedCount")
                VALUES
                    (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail,
                     @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp,
                     @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled,
                     @LockoutEnd, @LockoutEnabled, @AccessFailedCount)
                """;

            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken ct)
        {
            user.ConcurrencyStamp = Guid.NewGuid().ToString();

            const string sql = """
                UPDATE "AspNetUsers" SET
                    "UserName" = @UserName,
                    "NormalizedUserName" = @NormalizedUserName,
                    "Email" = @Email,
                    "NormalizedEmail" = @NormalizedEmail,
                    "EmailConfirmed" = @EmailConfirmed,
                    "PasswordHash" = @PasswordHash,
                    "SecurityStamp" = @SecurityStamp,
                    "ConcurrencyStamp" = @ConcurrencyStamp,
                    "PhoneNumber" = @PhoneNumber,
                    "PhoneNumberConfirmed" = @PhoneNumberConfirmed,
                    "TwoFactorEnabled" = @TwoFactorEnabled,
                    "LockoutEnd" = @LockoutEnd,
                    "LockoutEnabled" = @LockoutEnabled,
                    "AccessFailedCount" = @AccessFailedCount
                WHERE "Id" = @Id
                """;

            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken ct)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(@"DELETE FROM ""AspNetUsers"" WHERE ""Id"" = @Id", new { user.Id });
            return IdentityResult.Success;
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken ct)
        {
            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<ApplicationUser>(
                @"SELECT * FROM ""AspNetUsers"" WHERE ""Id"" = @Id", new { Id = userId });
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken ct)
        {
            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<ApplicationUser>(
                @"SELECT * FROM ""AspNetUsers"" WHERE ""NormalizedUserName"" = @NormalizedUserName",
                new { NormalizedUserName = normalizedUserName });
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.Id);

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken ct)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.NormalizedUserName);

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken ct)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        // ── IUserPasswordStore ────────────────────────────────────

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken ct)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.PasswordHash != null);

        // ── IUserEmailStore ───────────────────────────────────────

        public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken ct)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.Email);

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.EmailConfirmed);

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken ct)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken ct)
        {
            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<ApplicationUser>(
                @"SELECT * FROM ""AspNetUsers"" WHERE ""NormalizedEmail"" = @NormalizedEmail",
                new { NormalizedEmail = normalizedEmail });
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.NormalizedEmail);

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken ct)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        // ── IUserSecurityStampStore ───────────────────────────────

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken ct)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string?> GetSecurityStampAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.SecurityStamp);

        // ── IUserLockoutStore ─────────────────────────────────────

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.LockoutEnd);

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken ct)
        {
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken ct)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken ct)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.AccessFailedCount);

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.LockoutEnabled);

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken ct)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        // ── IUserTwoFactorStore ───────────────────────────────────

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken ct)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken ct)
            => Task.FromResult(user.TwoFactorEnabled);

        // ── IDisposable ───────────────────────────────────────────

        public void Dispose() { }
    }
}
