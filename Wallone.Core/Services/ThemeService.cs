namespace Wallone.Core.Services
{
    public class ThemeService
    {
        public static string GetCurrentName()
        {
            return SettingsService.Get().Current;
        }
    }
}