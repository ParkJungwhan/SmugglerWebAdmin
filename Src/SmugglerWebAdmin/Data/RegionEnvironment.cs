namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// 리전 내 환경(Dev/Beta/Alpha/Live 등) 정보.<br/>
    /// 스키마 관리용 EF 엔티티. CRUD는 Dapper(RegionService)를 통해 처리.
    /// </summary>
    public class RegionEnvironment
    {
        public int Id { get; set; }

        public int RegionId { get; set; }

        /// <summary>내부 식별자 (e.g., "dev")</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>화면 표시명 (e.g., "Dev")</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>Site 2 접속 URL. 미설정 시 빈 문자열.</summary>
        public string ToolUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
