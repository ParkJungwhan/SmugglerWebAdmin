using Microsoft.AspNetCore.Identity;
using SmugglerWebAdmin.Services;

namespace SmugglerWebAdmin.Data;

/// <summary>
/// 앱 최초 실행 시 슈퍼어드민 계정이 없으면 admin@admin / admin 계정을 생성합니다.
/// </summary>
public class AdminInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAdminService _adminService;

    public AdminInitializer(UserManager<ApplicationUser> userManager, IAdminService adminService)
    {
        _userManager = userManager;
        _adminService = adminService;
    }

    public async Task InitializeAsync()
    {
        const string adminUserName = "admin@admin";

        var existing = await _userManager.FindByNameAsync(adminUserName);
        if (existing is not null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = adminUserName,
            Email = adminUserName,
        };

        try
        {
            var result = await _userManager.CreateAsync(admin, "Adminadmin1!");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"슈퍼어드민 계정 생성 실패: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        catch (Exception ex)
        {
        }

        await _adminService.SetSuperAdminAsync(admin.Id, true);
        await _adminService.SetMustChangePasswordAsync(admin.Id, true);
    }
}