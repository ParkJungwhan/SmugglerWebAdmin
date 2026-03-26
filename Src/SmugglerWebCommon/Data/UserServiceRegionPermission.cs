namespace SmugglerWebCommon.Data
{
    /// <summary>
    /// L2 권한 — 리전 단위 개별 접근 권한.<br/>
    /// 이 권한이 있으면 해당 리전의 모든 환경에 접근 가능.<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper를 통해 처리.
    /// </summary>
    public class UserServiceRegionPermission
    {
        public int Id { get; set; }

        /// <summary>FK → AspNetUsers.Id</summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>FK → ServiceRegions.Id</summary>
        public int ServiceRegionId { get; set; }

        /// <summary>권한 부여 일시. DB 기본값 NOW().</summary>
        public DateTime CreatedAt { get; set; }
    }
}
