using SmugglerWebCommon.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// L1(Service)/L2(ServiceRegion)/L3(RegionEnvironment) 계층 데이터의 관리(CRUD) 인터페이스.<br/>
    /// 비활성 항목을 포함한 전체 데이터를 조회하며, 추가/수정을 제공합니다.
    /// </summary>
    public interface IRegionAdminService
    {
        // ── L1: Service ──────────────────────────────────────────────────────

        /// <summary>비활성 포함 전체 서비스 목록을 Id 순서로 반환합니다.</summary>
        Task<List<Service>> GetAllServicesAsync();

        /// <summary>새 서비스를 추가합니다. IsActive 기본값은 true입니다.</summary>
        Task AddServiceAsync(string name, string displayName);

        /// <summary>서비스 정보를 수정합니다.</summary>
        Task UpdateServiceAsync(int id, string name, string displayName, bool isActive);

        // ── L2: ServiceRegion ─────────────────────────────────────────────────

        /// <summary>비활성 포함, 지정 서비스에 속한 전체 리전 목록을 Id 순서로 반환합니다.</summary>
        Task<List<ServiceRegion>> GetAllRegionsByServiceAsync(int serviceId);

        /// <summary>새 리전을 추가합니다. IsActive 기본값은 true입니다.</summary>
        Task AddRegionAsync(int serviceId, string name, string displayName);

        /// <summary>리전 정보를 수정합니다.</summary>
        Task UpdateRegionAsync(int id, string name, string displayName, bool isActive);

        // ── L3: RegionEnvironment ─────────────────────────────────────────────

        /// <summary>비활성 포함, 지정 리전에 속한 전체 환경 목록을 Id 순서로 반환합니다.</summary>
        Task<List<RegionEnvironment>> GetAllEnvironmentsByRegionAsync(int serviceRegionId);

        /// <summary>새 환경을 추가합니다. IsActive 기본값은 true입니다.</summary>
        Task AddEnvironmentAsync(int serviceRegionId, string name, string displayName, string toolUrl);

        /// <summary>환경 정보를 수정합니다.</summary>
        Task UpdateEnvironmentAsync(int id, string name, string displayName, string toolUrl, bool isActive);
    }
}
