namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// L2 — 서비스 내 리전 (e.g., "Korea", "Japan").<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper를 통해 처리.
    /// </summary>
    public class ServiceRegion
    {
        public int Id { get; set; }

        /// <summary>FK → Services.Id</summary>
        public int ServiceId { get; set; }

        /// <summary>내부 식별자 (e.g., "kr")</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>화면 표시명 (e.g., "Korea")</summary>
        public string DisplayName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
