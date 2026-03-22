namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// L3 권한 — 환경 단위 개별 접근 권한.<br/>
    /// L1/L2 권한과 OR 조건으로 평가됩니다.<br/>
    /// 특정 리전의 일부 환경만 허용하려면 L2 권한 대신 L3 권한을 사용합니다.
    /// </summary>
    public class UserRegionEnvironmentPermission
    {
        public int Id { get; set; }

        /// <summary>FK → AspNetUsers.Id</summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>FK → RegionEnvironments.Id</summary>
        public int RegionEnvironmentId { get; set; }

        /// <summary>권한 부여 일시. DB 기본값 NOW().</summary>
        public DateTime CreatedAt { get; set; }
    }
}
