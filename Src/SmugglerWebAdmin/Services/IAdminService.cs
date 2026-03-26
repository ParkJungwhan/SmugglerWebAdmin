using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services;

/// <summary>어드민 전용 유저 관리 및 권한 일괄 저장 서비스.</summary>
public interface IAdminService
{
    /// <summary>전체 유저 목록을 가입일 내림차순으로 반환.</summary>
    Task<List<ApplicationUser>> GetAllUsersAsync();

    /// <summary>ID로 유저 단건 조회.</summary>
    Task<ApplicationUser?> GetUserByIdAsync(string userId);

    /// <summary>IsAdmin 플래그 설정.</summary>
    Task SetAdminAsync(string userId, bool isAdmin);

    /// <summary>IsSuperAdmin 플래그 설정.</summary>
    Task SetSuperAdminAsync(string userId, bool isSuperAdmin);

    /// <summary>MustChangePassword 플래그 설정.</summary>
    Task SetMustChangePasswordAsync(string userId, bool value);

    /// <summary>
    /// 유저의 L1/L2/L3 권한을 일괄 교체.<br/>
    /// 기존 권한을 모두 삭제한 뒤 새 권한을 삽입합니다.
    /// </summary>
    Task SetUserPermissionsAsync(
        string userId,
        IEnumerable<int> l1ServiceIds,
        IEnumerable<int> l2RegionIds,
        IEnumerable<int> l3EnvIds);
}
