using System;
using System.Collections.Generic;
using System.Text;
using WinDynamicDesktop.Core.Extension;

namespace WinDynamicDesktop.Core.Services
{
    // модель глобальных настроек
    public class AppSettings
    {
        //Путь до папки приложения
        public string AppPath { get; set; }

        //Путь до папки с темами
        public string ThemePath { get; set; }

        public string UseForFolders { get; set; }

        //Путь до настроек
        public string SettingsPath { get; set; }
    }

    public class AppSettingsService
    {
        private static AppSettings _appSettings = new AppSettings();

        public static AppSettings GetSettings()
        {
            return _appSettings;
        }

        public static string GetThemesLocation()
        {
            return _appSettings.ThemePath;
        }

        public static string GetUseForFolders()
        {
            return _appSettings.UseForFolders;
        }
        public static string GetAppLocation()
        {
            return _appSettings.AppPath;
        }
        public static void SetAppLocation(string path)
        {
            _appSettings.AppPath = path;
        }

        public static void SetThemesLocation(string path)
        {
            _appSettings.ThemePath = path;
        }

        public static void SetSettingsLocation(string path)
        {
            _appSettings.SettingsPath = path;
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
            _appSettings.UseForFolders = patten;
        }

        public static bool ExistDirectory(string path)
        {
            return path.ExistsDirectory();
        }
    }
}
