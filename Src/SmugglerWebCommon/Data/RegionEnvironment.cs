namespace SmugglerWebCommon.Data
{
    /// <summary>
    /// L3 — 리전 내 환경 (e.g., "Dev", "Beta", "Alpha", "Live").<br/>
    /// Select 페이지에서 클릭하면 Site 2를 새 탭으로 여는 단위.<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper를 통해 처리.
    /// </summary>
    public class RegionEnvironment
    {
        public int Id { get; set; }

        /// <summary>FK → ServiceRegions.Id</summary>
        public int ServiceRegionId { get; set; }

        /// <summary>내부 식별자 (e.g., "dev")</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>화면 표시명 (e.g., "Dev")</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>Site 2 접속 URL. 미설정 시 빈 문자열.</summary>
        public string ToolUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
