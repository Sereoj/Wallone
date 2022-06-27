using System.Diagnostics;
using System.IO;
using Wallone.Core.Extension;
using Wallone.Core.Interfaces;
using Wallone.Core.Models.App;

namespace Wallone.Core.Services.App
{
    // модель глобальных настроек
    public class AppSettingsRepository
    {
        private static readonly AppSettings AppSettings;
        private static readonly ISettings Settings;

        static AppSettingsRepository()
        {
            AppSettings = new AppSettings();
            Settings = new Settings();
        }

        public static AppSettings GetAppSettings()
        {
            return AppSettings;
        }

        public static ISettings GetSettings()
        {
            return Settings;
        }
        public class AppSettingsService
        {
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

            public static string GetApplicationPath()
            {
                return AppSettings.ApplicationPath;
            }

            public static string AppPath()
            {
                var processName = Process.GetCurrentProcess().ProcessName + ".exe";
                return Path.Combine(GetAppLocation(), processName);
            }
            public static void SetApplicationPath(string path)
            {
                AppSettings.ApplicationPath = path;
            }

            public static void SetAppDirectoryLocation(string path)
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

            public static void SetThemeConfigName(string name)
            {
                AppSettings.ThemeConfig = string.IsNullOrEmpty(name) ? "theme.json" : name;
            }

            public static string GetThemeConfigName()
            {
                return AppSettings.ThemeConfig;
            }

            public static bool ExistsFile(string path)
            {
                return path.ExistsFile();
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
}