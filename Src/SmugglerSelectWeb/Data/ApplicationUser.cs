using Microsoft.AspNetCore.Identity;

namespace SmugglerSelectWeb.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int AdminType { get; set; }
    }
}
