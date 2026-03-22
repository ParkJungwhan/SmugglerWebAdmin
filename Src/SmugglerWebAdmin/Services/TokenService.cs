using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// appsettings JwtSettings를 기반으로 JWT를 발행/검증합니다.<br/>
    /// 발행된 토큰은 Site 2(Tool) 접속 시 쿼리스트링으로 전달됩니다.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audienceTool;
        private readonly int _expirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            var section = configuration.GetSection("JwtSettings");
            _secretKey = section["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey not found.");
            _issuer = section["Issuer"] ?? "SmugglerWebAdmin";
            _audienceTool = section["AudienceTool"] ?? "SmugglerWebAdmin.Tool";
            _expirationMinutes = int.TryParse(section["ExpirationMinutes"], out var m) ? m : 30;
        }

        public string GenerateToken(ApplicationUser user, IEnumerable<int> permittedEnvironmentIds)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var envIds = string.Join(",", permittedEnvironmentIds);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("env_ids", envIds),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audienceTool,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out ClaimsPrincipal? principal)
        {
            principal = null;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audienceTool,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParams, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
