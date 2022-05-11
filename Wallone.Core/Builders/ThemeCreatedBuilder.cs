using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using Wallone.Core.Extension;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    //Созданная тема, где пользователь может скачать, установить, удалить.
    public class ThemeCreatedBuilder : IThemeCreatedBuilder
    {
        private readonly List<Image> images = new List<Image>();

        private List<Link> links;
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
                    Id = SinglePageService.GetID(),
                    Name = SinglePageService.GetHeader(),
                    User = SinglePageService.GetUser(),
                    Preview = Path.Combine(ThemePath, ThemeThumbFileName),
                    Images = images,
                    Brand = SinglePageService.GetBrand(),
                    Categories = SinglePageService.GetCategories(),
                    Description = SinglePageService.GetDescription(),
                    Created_at = SinglePageService.GetDate(),
                    Resolution = "Не доступно, отвечает за пользовательские разрешения изображений для мониторов",
                    HashCode = null
                };
            return this;
        }

        public ThemeCreatedBuilder Save()
        {
            if (ThemePath == null || Theme == null) return this;
            if (AppSettingsService.ExistDirectory(ThemePath))
            {
                var file = Path.Combine(ThemePath, AppSettingsService.GetThemeConfigName());
                File.WriteAllText(file, JsonConvert.SerializeObject(Theme, Formatting.Indented));
            }

            return this;
        }

        public ThemeCreatedBuilder SetImages(List<Link> images)
        {
            this.images.Clear();
            if (images != null)
            {
                foreach (var item in images)
                    this.images.Add(new Image
                    {
                        id = item.id,
                        times = item.name,
                        location = UriHelper.GetUri(item.location, ThemePath, "?")
                    });
            }

            links = images;

            return this;
        }

        public bool ValidateConfig()
        {
            var configFile = GetConfigPath();

            if (configFile.ExistsFile())
            {
                var jsonText = File.ReadAllText(configFile);
                if (!JsonHelper.IsValidJson(jsonText))
                {
                    return false;
                }

                return true;

            }
            return false;
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
                }

            return this;
        }

        public async Task PreviewDownloadAsync()
        {
            if (AppConvert.Revert(ThemeHasDownloaded))
            {
                var preview = SinglePageService.GetPreview();
                var uri = UriHelper.Get(preview).LocalPath;

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
            if (AppSettingsService.GetUseForFolders() == "name")
                ThemeHasDownloaded = AppSettingsService.ExistDirectory(GetThemePath());

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

            if (AppSettingsService.GetUseForFolders() == "name")
                if (ThemePath != null && AppSettingsService.ExistDirectory(ThemePath))
                    return true;

            return false;
        }

        public ThemeCreatedBuilder ExistOrCreateDirectory()
        {
            if (AppConvert.Revert(Exist()))
                AppSettingsService.CreateDirectory(ThemePath);
            return this;
        }

        public ThemeCreatedBuilder SetName(string name)
        {
            ThemeName = name;
            Trace.WriteLine("SetName:" + name);
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
            var themes = AppSettingsService.GetThemesLocation();
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
            return Path.Combine(GetThemePath(), AppSettingsService.GetThemeConfigName());
        }

        public SinglePage GetModelFromFile()
        {
            var configFile = GetConfigPath();

            if (ValidateConfig())
            {
                var jsonText = File.ReadAllText(configFile);
                return JsonConvert.DeserializeObject<SinglePage>(jsonText);
            }

            return null;
        }

        public Theme GetThemeModelFromFile()
        {
            var configFile = GetConfigPath();

            if (ValidateConfig())
            {
                var jsonText = File.ReadAllText(configFile);
                Theme = JsonConvert.DeserializeObject<Theme>(jsonText);
                return GetModel();
            }

            return null;
        }
    }
}