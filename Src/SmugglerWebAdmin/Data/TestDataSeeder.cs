using Microsoft.AspNetCore.Identity;

namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// 개발/테스트용 가상 유저 100명을 생성합니다.<br/>
    /// pjh0001@gmail.com ~ pjh0100@gmail.com / 암호: Platform1!<br/>
    /// 이미 존재하는 경우 생성을 건너뜁니다.
    /// </summary>
    public class TestDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TestDataSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            const string password = "Platform1!";

            for (int i = 1; i <= 100; i++)
            {
                var email = $"pjh{i:D4}@gmail.com";

                if (await _userManager.FindByEmailAsync(email) is not null)
                    continue;

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    NormalizedUserName = email.ToUpperInvariant(),
                    NormalizedEmail = email.ToUpperInvariant(),
                    EmailConfirmed = true,
                    IsActive = true,
                };

                await _userManager.CreateAsync(user, password);
            }
        }
    }
}
