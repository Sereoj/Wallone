﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wallone.Core.Extension;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Pages;

namespace Wallone.Core.Builders
{
    //Созданная тема, где пользователь может скачать, установить, удалить.
    public class ThemeCreatedBuilder : IThemeCreatedBuilder
    {
        private readonly List<Image> images = new List<Image>();

        private List<Image> links;
        private static bool ThemeHasDownloaded { get; set; }
        private static bool ThemeHasInstalled { get; set; }
        private static bool ThemeHasFavorited { get; set; }
        private static bool ThemeHasLiked { get; set; }
        private static string ThemeName { get; set; }
        private static string ThemePath { get; set; }

        private static string ThemeThumbFileName { get; set; } = "thumb.jpg";

        private Theme Theme { get; set; }

        //Удалить тему
        public ThemeCreatedBuilder Remove()
        {
            if (ThemeHasDownloaded)
                if (AppSettingsRepository.AppSettingsService.GetUseForFolders() == "name" && ThemePath != null)
                    if (AppSettingsRepository.AppSettingsService.ExistDirectory(ThemePath))
                        AppSettingsRepository.AppSettingsService.RemoveDirectory(ThemePath);

            return this;
        }

        //Скачать и установить
        public ThemeCreatedBuilder DownloadAndInstall()
        {
            return this;
        }

        //Установить атрибуты к папке
        public ThemeCreatedBuilder SetAttributeDirectory()
        {
            return this;
        }

        //Установить атрибуты к файлам
        public ThemeCreatedBuilder SetAttributeFiles()
        {
            return this;
        }

        ThemeCreatedBuilder IThemeCreatedBuilder.Download()
        {
            return this;
        }

        public ThemeCreatedBuilder CreateModel()
        {
            if (AppConvert.Revert(ThemeHasDownloaded))
                Theme = new Theme
                {
                    Uuid = SinglePageService.GetID(),
                    Name = SinglePageService.GetHeader(),
                    User = SinglePageService.GetUser(),
                    Preview = Path.Combine(ThemePath, ThemeThumbFileName),
                    Images = images,
                    Created_at = SinglePageService.GetDate(),
                    Resolution = "Не доступно, отвечает за пользовательские разрешения изображений для мониторов",
                };
            return this;
        }

        public ThemeCreatedBuilder Save()
        {
            if (ThemePath == null || Theme == null) return this;
            if (AppSettingsRepository.AppSettingsService.ExistDirectory(ThemePath))
            {
                var file = Path.Combine(ThemePath, AppSettingsRepository.AppSettingsService.GetThemeConfigName());
                File.WriteAllText(file, JsonConvert.SerializeObject(Theme, Formatting.Indented));
            }

            return this;
        }

        public ThemeCreatedBuilder SetLinksForDownload(List<Image> images)
        {
            if (images != null)
            {
                this.images.Clear();
                links = images;
            }
            return this;
        }

        public bool ValidateConfig()
        {
            var configFile = GetConfigPath();

            if (!configFile.ExistsFile()) return false;
            var jsonText = File.ReadAllText(configFile);
            return JsonHelper.IsValidJson(jsonText);
        }

        private async Task DownloadTask(string uri, string filename)
        {
            var wb = new WebClient();
            await wb.DownloadFileTaskAsync(UriHelper.Get(uri), filename);
        }

        // Скачать тему
        public async Task<ThemeCreatedBuilder> ImageDownload()
        {
            if (AppConvert.Revert(ThemeHasDownloaded) && links != null)
                foreach (var item in links)
                {
                    var path = UriHelper.GetUri(item.location, ThemePath, "?");

                    await DownloadTask(item.location, path);
                    await Task.Delay(100);

                    images.Add(new Image()
                    {
                        id = item.id,
                        times = item.times,
                        location = path
                    });
                }

            return this;
        }

        public async Task PreviewDownloadAsync()
        {
            if (AppConvert.Revert(ThemeHasDownloaded))
            {
                var preview = SinglePageService.GetPreview();
                var uri = UriHelper.Get(preview).OriginalString;

                await DownloadTask(uri, Path.Combine(ThemePath, ThemeThumbFileName));
                await Task.Delay(100);
            }
        }

        public ThemeCreatedBuilder HasNotLiked(bool value)
        {
            //ThemeHasLiked = value != true;
            ThemeHasLiked = value;
            return this;
        }

        public ThemeCreatedBuilder HasFavorited(bool value)
        {
            //ThemeHasFavorited = value != true;
            ThemeHasFavorited = value;
            return this;
        }

        public ThemeCreatedBuilder HasInstalled(bool value)
        {
            //ThemeHasInstalled = value != true;
            ThemeHasInstalled = value;
            return this;
        }


        public ThemeCreatedBuilder HasDownloaded()
        {
            if (AppSettingsRepository.AppSettingsService.GetUseForFolders() == "name")
                ThemeHasDownloaded = AppSettingsRepository.AppSettingsService.ExistDirectory(GetThemePath());

            return this;
        }

        public Theme GetModel()
        {
            return Theme;
        }

        public void Build()
        {
        }

        public bool Exist()
        {
            ThemePath = GetThemePath();

            if (AppSettingsRepository.AppSettingsService.GetUseForFolders() == "name")
                if (ThemePath != null && AppSettingsRepository.AppSettingsService.ExistDirectory(ThemePath))
                    return true;

            return false;
        }

        public ThemeCreatedBuilder ExistOrCreateDirectory()
        {
            if (AppConvert.Revert(Exist()))
                AppSettingsRepository.AppSettingsService.CreateDirectory(ThemePath);
            return this;
        }

        public ThemeCreatedBuilder SetName(string name)
        {
            ThemeName = AppFormat.Format(name);
            return this;
        }

        public ThemeCreatedBuilder SetThumbName(string name)
        {
            ThemeThumbFileName = name;
            return this;
        }

        public string GetName()
        {
            return ThemeName;
        }

        public string GetThemePath()
        {
            var themes = AppSettingsRepository.AppSettingsService.GetThemesLocation();
            return ThemeName != null ? Path.Combine(themes, ThemeName) : null;
        }

        public bool GetHasNotDownloaded()
        {
            return AppConvert.Revert(ThemeHasDownloaded);
        }

        public bool GetHasNotInstalled()
        {
            return AppConvert.Revert(ThemeHasInstalled);
        }

        public bool GetHasNotFavorited()
        {
            return AppConvert.Revert(ThemeHasFavorited);
        }

        public bool GetHasNotLiked()
        {
            return AppConvert.Revert(ThemeHasLiked);
        }

        public string GetConfigPath()
        {
            var themePath = GetThemePath();
            var themeConfig = AppSettingsRepository.AppSettingsService.GetThemeConfigName();

            if(themePath != null && themeConfig != null)
            {
                return Path.Combine(themePath, themeConfig);
            }
            return null;
        }

        public SinglePage GetModelFromFile()
        {
            var configFile = GetConfigPath();

            if (ValidateConfig())
            {
                var jsonText = File.ReadAllText(configFile);

                //Json<SinglePage>.Decode
                return Json<SinglePage>.Decode(jsonText);
            }

            return null;
        }

        public Theme GetThemeModelFromFile()
        {
            var configFile = GetConfigPath();

            if (ValidateConfig())
            {
                var jsonText = File.ReadAllText(configFile);
                Theme = Json<Theme>.Decode(jsonText);
                return GetModel();
            }

            return null;
        }
    }
}