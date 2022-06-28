using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Services
{
    public class ThemeRepository
    {
        private static Theme themeModel;

        public class ThemeService
        {
            private static string themeName;
            private static int themeImageId;

            public static string GetCurrentThemeName()
            {
                return themeName;
            }

            public static void SetCurrentName(string name)
            {
                themeName = AppFormat.Format(name);

                new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder()
                    .SetTheme(name)
                    .Build();
            }

            public static string GetPath()
            {
                return Path.Combine(AppSettingsRepository.AppSettingsService.GetThemesLocation(), themeName);
            }

            public static string GetPathConfig()
            {
                return Path.Combine(GetPath(), AppSettingsRepository.AppSettingsService.GetThemeConfigName());
            }

            public static void Set(Theme theme)
            {
                if (theme != null)
                {
                    SetCurrentName(theme.Name);
                }
                themeModel = theme;
            }

            public static Theme Get()
            {
                return themeModel;
            }

            public static void SetImageId(string imageId)
            {
                themeImageId = int.Parse(imageId);
            }
            public static int GetImageId()
            {
                return themeImageId;
            }

            public static bool ValidateImageId(int imageId)
            {
                return themeModel.Images.Exists(image => int.Parse(image.id) == imageId);
            }

            public static string GetPrevious()
            {

                var maxId = int.Parse(themeModel.Images.OrderByDescending(image => image.id).FirstOrDefault()!.id);
                var minId = int.Parse(themeModel.Images.OrderBy(image => image.id).FirstOrDefault()!.id);

                if (ValidateImageId(themeImageId))
                {
                    var currentId = int.Parse(themeModel.Images.FirstOrDefault(image => int.Parse(image.id) == themeImageId)!.id);

                    themeImageId = minId < currentId ? currentId - 1 : maxId;
                    return GetCurrentImage();
                }
                return null;
            }

            public static string GetCurrentImage()
            {
                if (ValidateImageId(themeImageId))
                {
                    var imageLocation = themeModel.Images
                        .FirstOrDefault(image => int.Parse(image.id) == themeImageId)?.location;
                    LoggerService.Log(typeof(ThemeService), $"GetImageId: {ThemeRepository.ThemeService.GetImageId()}");
                    
                    return AppSettingsRepository.AppSettingsService.ExistsFile(imageLocation) ? imageLocation : null;
                }
                LoggerService.Log(typeof(ThemeService), $"GetImageId: {ThemeRepository.ThemeService.GetImageId()}");
                LoggerService.Log(typeof(ThemeService), $"GetCurrentImage: null");
                return null;
            }

            public static string GetCurrentImage(int themeId)
            {
                if (ValidateImageId(themeId))
                {
                    var imageLocation = themeModel.Images
                        .FirstOrDefault(image => int.Parse(image.id) == themeId)?.location;

                    LoggerService.Log(typeof(ThemeService), $"GetImageId: {ThemeRepository.ThemeService.GetImageId()}");

                    return AppSettingsRepository.AppSettingsService.ExistsFile(imageLocation) ? imageLocation : null;
                }
                LoggerService.Log(typeof(ThemeService), $"GetImageId: {ThemeRepository.ThemeService.GetImageId()}");
                LoggerService.Log(typeof(ThemeService), $"GetCurrentImage: null");
                return null;
            }

            public static string GetNext()
            {
                var maxId = int.Parse(themeModel.Images.OrderByDescending(image => image.id).FirstOrDefault()!.id);
                var minId = int.Parse(themeModel.Images.OrderBy(image => image.id).FirstOrDefault()!.id);

                if (ValidateImageId(themeImageId))
                {
                    var currentId = int.Parse(themeModel.Images.FirstOrDefault(image => int.Parse(image.id) == themeImageId)!.id);

                    themeImageId = maxId > currentId ? currentId + 1 : minId;

                    return GetCurrentImage();
                }
                return null;
            }
        }

        public static void Load()
        {
            try
            {
                var jsonText = File.ReadAllText(ThemeService.GetPathConfig());
                themeModel = JsonConvert.DeserializeObject<Theme>(jsonText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void Save()
        {
            try
            {
                var file = ThemeService.GetPathConfig();
                File.WriteAllText(file, JsonConvert.SerializeObject(themeModel, Formatting.Indented));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}