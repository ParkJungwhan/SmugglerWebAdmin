namespace SmugglerWebAdmin.Services;

/// <summary>테마 모드.</summary>
public enum ThemeMode
{
    Light,
    Dark,
    System
}

/// <summary>애플리케이션 전체 테마 상태를 관리하는 서비스.</summary>
public class ThemeService
{
    /// <summary>현재 테마 모드. 기본값은 System.</summary>
    public ThemeMode Mode { get; private set; } = ThemeMode.System;

    /// <summary>테마가 변경되면 발생하는 이벤트.</summary>
    public event Action? OnChange;

    /// <summary>테마 모드를 설정하고 변경 이벤트를 발생시킨다.</summary>
    public void Set(ThemeMode mode)
    {
        Mode = mode;
        OnChange?.Invoke();
    }
}
