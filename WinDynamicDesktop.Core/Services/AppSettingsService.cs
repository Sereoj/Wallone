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

        public static string GetAppLocation()
        {
            return _appSettings.AppPath;
        }
        public static void SetAppLocation(string path)
        {
            _appSettings.AppPath = path;
        }

        public static void CreateDirectory(string path)
        {
            path.CreateDirectory();
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
            _appSettings.AppPath = patten;
        }

        public static bool ExistDirectory(string path)
        {
            return path.ExistsDirectory();
        }
    }
}
