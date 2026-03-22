namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// 사용자별 리전 환경 접근 권한.<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper(RegionService)를 통해 처리.
    /// </summary>
    public class UserRegionPermission
    {
        public int Id { get; set; }

        /// <summary>FK → AspNetUsers.Id</summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>FK → RegionEnvironments.Id</summary>
        public int RegionEnvironmentId { get; set; }
    }
}
