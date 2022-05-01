using Wallone.Core.Extension;

namespace Wallone.Core.Services
{
    // модель глобальных настроек

    public class AppSettingsService
    {
        private static readonly AppSettings AppSettings;

        static AppSettingsService()
        {
            AppSettings = new AppSettings();
        }

        public static AppSettings GetSettings()
        {
            return AppSettings;
        }

        public static string GetThemesLocation()
        {
            return AppSettings.ThemePath;
        }

        public static string GetUseForFolders()
        {
            return AppSettings.UseForFolders;
        }

        public static string GetAppLocation()
        {
            return AppSettings.AppPath;
        }

        public static void SetAppLocation(string path)
        {
            AppSettings.AppPath = path;
        }

        public static void SetThemesLocation(string path)
        {
            AppSettings.ThemePath = path;
        }

        public static void SetSettingsLocation(string path)
        {
            AppSettings.SettingsPath = path;
        }

        public static void CreateDirectory(string path)
        {
            path.CreateDirectory();
        }

        public static void RemoveDirectory(string path)
        {
            path.DeleteDirectory();
        }

        public static void SetThemeNameForFolders(string patten)
        {
            switch (patten)
            {
                case "name":
                case "id":
                    break;
                default:
                    patten = "name";
                    break;
            }

            AppSettings.UseForFolders = patten;
        }

        public static bool ExistDirectory(string path)
        {
            return path.ExistsDirectory();
        }
    }
}