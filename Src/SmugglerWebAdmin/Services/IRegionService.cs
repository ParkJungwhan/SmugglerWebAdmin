using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// 리전/환경 조회 및 사용자 권한 조회 인터페이스.<br/>
    /// 쿼리는 Dapper를 통해 처리됩니다.
    /// </summary>
    public interface IRegionService
    {
        /// <summary>IsActive가 true인 모든 리전을 Id 순서로 반환합니다.</summary>
        Task<IEnumerable<Region>> GetAllActiveRegionsAsync();

        /// <summary>지정 리전에 속하며 IsActive가 true인 환경 목록을 반환합니다.</summary>
        Task<IEnumerable<RegionEnvironment>> GetEnvironmentsByRegionAsync(int regionId);

        /// <summary>사용자가 접근 권한을 가진 RegionEnvironmentId 목록을 반환합니다.</summary>
        Task<IEnumerable<int>> GetPermittedEnvironmentIdsAsync(string userId);
    }
}
