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
        private List<Images> Images;
        private static bool ThemeHasNotInstalled { get; set; }
        private static bool ThemeHasNotFavorited { get; set; }
        private static bool ThemeHasNotLiked { get; set; }
        private static string ThemeName { get; set; }
        private static string ThemePath { get; set; }

        //Удалить тему
        public ThemeCreatedBuilder Remove()
        {
            if (ThemeHasNotInstalled == false)
                if (AppSettingsService.GetUseForFolders() == "name" && ThemePath != null)
                    if (AppSettingsService.ExistDirectory(ThemePath))
                        AppSettingsService.RemoveDirectory(ThemePath);

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

        ThemeCreatedBuilder IThemeCreatedBuilder.Download()
        {
            return this;
        }

        public ThemeCreatedBuilder CreateModel(List<Images> images)
        {
            if (Images == null) Images = images;
            return this;
        }

        // Скачать тему
        public async Task<ThemeCreatedBuilder> Download()
        {
            if (ThemeHasNotInstalled)
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
                    // т.е тема уже установлена
                    ThemeHasNotInstalled = false;
                else
                    ThemeHasNotInstalled = true;
            }

            return this;
        }

        public ThemeCreatedBuilder Build()
        {
            return this;
        }

        public ThemeCreatedBuilder ExistOrCreateDirectory()
        {
            if (ThemeHasNotInstalled)
                if (AppSettingsService.GetUseForFolders() == "name" && ThemePath != null)
                    if (!AppSettingsService.ExistDirectory(ThemePath))
                        AppSettingsService.CreateDirectory(ThemePath);
            return this;
        }

        public ThemeCreatedBuilder SetName(string name)
        {
            ThemeName = !string.IsNullOrEmpty(name) ? name : "default";
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
    }
}