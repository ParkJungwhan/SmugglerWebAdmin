using Microsoft.AspNetCore.Identity;

namespace SmugglerWebAdmin.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>계정 등록 일시. DB 기본값 NOW().</summary>
        public DateTime CreatedAt { get; set; }
    }

}
