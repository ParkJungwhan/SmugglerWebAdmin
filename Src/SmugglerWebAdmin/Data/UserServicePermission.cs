namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// L1 권한 — 서비스 단위 접근 권한.<br/>
    /// 이 권한이 있으면 해당 서비스의 모든 리전/환경에 접근 가능.<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper를 통해 처리.
    /// </summary>
    public class UserServicePermission
    {
        public int Id { get; set; }

        /// <summary>FK → AspNetUsers.Id</summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>FK → Services.Id</summary>
        public int ServiceId { get; set; }

        /// <summary>권한 부여 일시. DB 기본값 NOW().</summary>
        public DateTime CreatedAt { get; set; }
    }
}
