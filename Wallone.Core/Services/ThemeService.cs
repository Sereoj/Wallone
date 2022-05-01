namespace Wallone.Core.Services
{
    public class ThemeService
    {
        public static string GetCurrentName()
        {
            return SettingsService.Get().Current;
        }

        public static void SetCurrentName(string name)
        {
            SettingsService.Get().Current = name;
            SettingsService.Save();
        }
    }
}