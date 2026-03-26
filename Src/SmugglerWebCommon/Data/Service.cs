namespace SmugglerWebCommon.Data
{
    /// <summary>
    /// L1 — 서비스 (최상위 그룹).<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper를 통해 처리.
    /// </summary>
    public class Service
    {
        public int Id { get; set; }

        /// <summary>내부 식별자 (e.g., "service1")</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>화면 표시명 (e.g., "Service 1")</summary>
        public string DisplayName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
