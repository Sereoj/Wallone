using System;
using System.IO;
using WinDynamicDesktop.Core.Interfaces;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class AppConvert
    {
        public static bool StringToBool(string value)
        {
            return value == "true" ? true : false;
        }

        public static string BoolToString(bool value)
        {
            return value == true ? "true" : "false";
        }

        public static bool Revert(bool value)
        {
            return value != true;
        }
    }
    public class ThemeCreated
    {

    }

    //Созданная тема, где пользователь может скачать, установить, удалить.
    public class ThemeCreatedBuilder : IThemeCreatedBuilder
    {
        private static bool ThemeHasNotInstalled { get; set; } = false;
        private static bool ThemeHasNotFavorited { get; set; } = false;
        private static bool ThemeHasNotLiked { get; set; } = false;
        private static string ThemeName { get; set; }
        private static string ThemePath { get; set; }
        public ThemeCreatedBuilder CreateModel()
        {
            return this;
        }
        // Скачать тему
        public ThemeCreatedBuilder Download()
        {
            return this;
        }

        //Удалить тему
        public ThemeCreatedBuilder Remove()
        {
            if (ThemeHasNotInstalled == false)
            {
                if (AppSettingsService.GetUseForFolders() == "name" && ThemePath != null)
                {
                    if (AppSettingsService.ExistDirectory(ThemePath))
                    {
                        AppSettingsService.RemoveDirectory(ThemePath);
                    }
                }
            }
            return this;
        }

        //Скачать и установить
        public ThemeCreatedBuilder DownloadAndInstall()
        {
            return this;
        }

        //Установить атрибуты к папке
        public ThemeCreatedBuilder SetAttibuteDirectory()
        {
            return this;
        }

        //Установить атрибуты к файлам
        public ThemeCreatedBuilder SetAttibuteFiles()
        {
            return this;
        }

        public ThemeCreatedBuilder HasNotLiked(bool value)
        {
            ThemeHasNotLiked = value != true;
            return this;
        }

        public ThemeCreatedBuilder HasNotFavorited(bool value)
        {
            ThemeHasNotFavorited = value != true;
            return this;
        }

        public ThemeCreatedBuilder HasNotInstalled(bool value)
        {
            var themes = AppSettingsService.GetThemesLocation();
            if(AppSettingsService.GetUseForFolders() == "name")
            {
                ThemePath = Path.Combine(themes, ThemeName);
                if (value == true || AppSettingsService.ExistDirectory(ThemePath))
                {
                    // т.е тема уже установлена
                    ThemeHasNotInstalled = false;
                }
                else
                {
                    ThemeHasNotInstalled = true;
                }
            }
            return this;
        }

        public ThemeCreatedBuilder Build()
        {
            return this;
        }

        public ThemeCreatedBuilder ExistOrCreateDirectory()
        {
            if (ThemeHasNotInstalled == true)
            {
                if (AppSettingsService.GetUseForFolders() == "name" && ThemePath != null)
                {
                    if(!AppSettingsService.ExistDirectory(ThemePath))
                    {
                        AppSettingsService.CreateDirectory(ThemePath);
                    }
                }
            }
            return this;
        }

        public ThemeCreatedBuilder SetName(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                ThemeName = name;
            }
            return this;
        }

        public bool GetHasNotInstalled()
        {
            return ThemeHasNotInstalled;
        }
        public bool GetHasNotFavorited()
        {
            return ThemeHasNotFavorited;
        }
        public bool GetHasNotLiked()
        {
            return ThemeHasNotLiked;
        }
    }
}