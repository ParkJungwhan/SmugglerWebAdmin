namespace SmugglerWebCommon.Services
{
    /// <summary>Light / Dark / System 테마 모드.</summary>
    public enum ThemeMode { Light, Dark, System }

    /// <summary>
    /// UI 테마 상태를 공유하는 Scoped 서비스.<br/>
    /// localStorage 저장/로드는 호출 측(컴포넌트)에서 처리합니다.
    /// </summary>
    public class ThemeService
    {
        public ThemeMode Mode { get; private set; } = ThemeMode.System;

        public event Action? OnChange;

        public void Set(ThemeMode mode)
        {
            Mode = mode;
            OnChange?.Invoke();
        }
    }
}
