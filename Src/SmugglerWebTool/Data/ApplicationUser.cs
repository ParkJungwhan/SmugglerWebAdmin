using Microsoft.AspNetCore.Identity;

namespace SmugglerWebTool.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int AdminType { get; set; }
    }
}
