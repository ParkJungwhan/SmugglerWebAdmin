using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// 서비스/리전/환경 조회 및 사용자 권한 조회 인터페이스.<br/>
    /// 쿼리는 Dapper를 통해 처리됩니다.
    /// </summary>
    public interface IRegionService
    {
        /// <summary>IsActive가 true인 모든 서비스(L1)를 Id 순서로 반환합니다.</summary>
        Task<IEnumerable<Service>> GetAllActiveServicesAsync();

        /// <summary>지정 서비스에 속하며 IsActive가 true인 리전(L2) 목록을 반환합니다.</summary>
        Task<IEnumerable<ServiceRegion>> GetRegionsByServiceAsync(int serviceId);

        /// <summary>지정 리전에 속하며 IsActive가 true인 환경(L3) 목록을 반환합니다.</summary>
        Task<IEnumerable<RegionEnvironment>> GetEnvironmentsByRegionAsync(int serviceRegionId);

        /// <summary>사용자가 L1 권한을 가진 ServiceId 목록을 반환합니다.</summary>
        Task<IEnumerable<int>> GetPermittedServiceIdsAsync(string userId);

        /// <summary>사용자가 L2 권한을 가진 ServiceRegionId 목록을 반환합니다.</summary>
        Task<IEnumerable<int>> GetPermittedServiceRegionIdsAsync(string userId);

        /// <summary>사용자가 L3 권한을 가진 RegionEnvironmentId 목록을 반환합니다.</summary>
        Task<IEnumerable<int>> GetPermittedEnvironmentIdsAsync(string userId);
    }
}
