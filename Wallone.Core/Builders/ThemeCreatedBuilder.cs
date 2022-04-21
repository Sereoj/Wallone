using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    //Созданная тема, где пользователь может скачать, установить, удалить.
    public class ThemeCreatedBuilder : IThemeCreatedBuilder
    {
        private static bool ThemeHasNotInstalled { get; set; } = false;
        private static bool ThemeHasNotFavorited { get; set; } = false;
        private static bool ThemeHasNotLiked { get; set; } = false;
        private static string ThemeName { get; set; }
        private static string ThemePath { get; set; }

        private List<Images> Images;
        public ThemeCreatedBuilder CreateModel(List<Images> images)
        {
            if (Images == null)
            {
                Images = images;
            }
            return this;
        }
        // Скачать тему
        public async Task<ThemeCreatedBuilder> Download()
        {
            if (ThemeHasNotInstalled == true)
            {
                foreach (var item in Images)
                {
                    var wb = new WebClient();

                    var replaced_link = item.location.Replace("images/carousel_", "uploads/");
                    var filename = item.location.Replace("/public/storage/images/carousel_", "");

                    var currentWallpaper = Path.Combine(ThemePath, filename);
                    await wb.DownloadFileTaskAsync(UriHelper.Get(replaced_link), currentWallpaper);
                }
                await Task.CompletedTask;
            }
            return this;
        }

        //Удалить тему
        public ThemeCreatedBuilder Remove()
        {
            try
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
            }
            catch (Exception)
            {

                throw;
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
            if (AppSettingsService.GetUseForFolders() == "name")
            {
                ThemePath = Path.Combine(themes, ThemeName);
                if (value && AppSettingsService.ExistDirectory(ThemePath))
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
                    if (!AppSettingsService.ExistDirectory(ThemePath))
                    {
                        AppSettingsService.CreateDirectory(ThemePath);
                    }
                }
            }
            return this;
        }

        public ThemeCreatedBuilder SetName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                ThemeName = name;
            }
            return this;
        }

        public string GetThemePath()
        {
            return ThemePath;
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

        ThemeCreatedBuilder IThemeCreatedBuilder.Download()
        {
            return this;
        }
    }
}