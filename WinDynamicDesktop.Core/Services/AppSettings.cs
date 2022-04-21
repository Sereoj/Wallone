﻿namespace WinDynamicDesktop.Core.Services
{
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
}