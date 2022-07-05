using System;
using System.Collections.Generic;
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
    public class ThemeCollection
    {
        public int Id { get; set; }
        public Times Phase { get; set; }
        public string Location { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set; }
    }
    public class ThemeRepository
    {
        private static Theme themeModel;
        private static List<ThemeCollection> collections = new List<ThemeCollection>();

        public class Collection
        {
            public static List<ThemeCollection> Get()
            {
                return collections;
            }

            public static void Add(ThemeCollection theme)
            {
                if(!collections.Exists(th => th.Id == theme.Id))
                {
                    collections.Add(theme);
                }
                else
                {
                    LoggerService.Log(typeof(Collection), "Данная модель уже существует с таким Id\nВозможно theme.json собран неправильно", Message.Warn);
                }
            }

            public static void Remove(ThemeCollection theme)
            {
                if (collections.Exists(th => th.Id == theme.Id))
                {
                    collections.Remove(theme);
                }
                else
                {
                    LoggerService.Log(typeof(Collection), $"Данная модель уже не существует с таким Id {theme.Id}. Возможно theme.json собран неправильно", Message.Warn);
                }
            }

            public static void Clear()
            {
                if(collections == null)
                {
                    LoggerService.Log(typeof(Collection), "Невозможно очистить коллекцию, поскольку она уже пуста!", Message.Warn);
                }
                else
                {
                    collections.Clear();
                }
            }
        }
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

            /// <summary>
            /// Создается промежуток времени по следующего изображения
            /// </summary>
            /// <param name="date2">День</param>
            /// <param name="date1">Утро</param>
            /// <param name="count">5</param>
            /// <returns></returns>
            public static TimeSpan SunSpanImages(DateTime date2, DateTime date1, int count)
            {
                if (count == 0)
                {
                    LoggerService.Log(typeof(ThemeService), $"Число не должно быть равным 0", Message.Warn);
                    count = 1;
                }
                return (date2 - date1) / count;
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
                    themeModel = theme;
                }
                else
                {
                    LoggerService.Log(typeof(ThemeService), $"Модель не должна быть пуста. Необходимо заново переустановить тему!", Message.Warn);
                }

            }

            public static void SetImageId(int imageId)
            {
                themeImageId = imageId;
            }
            public static int GetImageId()
            {
                return themeImageId;
            }

            public static bool ValidateImageId(int imageId)
            {
                return themeModel.Images.Exists(image => image.id == imageId);
            }

            public static string GetCurrentImage(ThemeCollection themeCollection)
            {
                if (themeCollection != null)
                {
                    themeImageId = themeCollection.Id;
                    var imageLocation = themeCollection.Location;
                    return AppSettingsRepository.AppSettingsService.ExistsFile(imageLocation) ? imageLocation : null;
                }
                LoggerService.Log(typeof(ThemeService), "Не удалось установить изображение", Message.Warn);
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

        public static Theme Get()
        {
            return themeModel;
        }

        public static List<Image> GetImages()
        {
            return themeModel.Images.OrderBy(image => image.times == Times.Dawn).ToList();
        }

        public static List<Image> GetImagesOrderBy()
        {
            return themeModel.Images.OrderBy(image => image.id).ToList();
        }
        public static List<Image> GetImagesOrderBy(Times times)
        {
            return themeModel.Images.OrderBy(image => image.times == times).ToList();
        }
    }
}