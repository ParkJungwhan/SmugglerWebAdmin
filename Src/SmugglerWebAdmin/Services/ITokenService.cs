using System.Security.Claims;
using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// Site 2(Tool) 연동용 JWT 토큰 발행/검증 인터페이스.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 사용자 정보와 허용된 환경 ID 목록을 클레임으로 담아 JWT를 발행합니다.
        /// </summary>
        string GenerateToken(ApplicationUser user, IEnumerable<int> permittedEnvironmentIds);

        /// <summary>
        /// JWT 토큰의 유효성을 검사하고, 성공 시 ClaimsPrincipal을 반환합니다.
        /// </summary>
        bool ValidateToken(string token, out ClaimsPrincipal? principal);
    }
}
