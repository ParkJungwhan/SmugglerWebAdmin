using Microsoft.AspNetCore.Identity;

namespace SmugglerWebAdmin.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>계정 등록 일시. DB 기본값 NOW().</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>어드민 권한. 유저 목록 조회 및 권한 편집 가능.</summary>
        public bool IsAdmin { get; set; }

        /// <summary>슈퍼어드민 권한. 어드민 지정/해제 포함 모든 관리 기능 사용 가능.</summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>강제 비밀번호 변경 플래그. true이면 로그인 후 비밀번호 변경 페이지로 이동.</summary>
        public bool MustChangePassword { get; set; }

        /// <summary>계정 활성화 여부. false이면 로그인 불가.</summary>
        public bool IsActive { get; set; } = true;
    }

}
