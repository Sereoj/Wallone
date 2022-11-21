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
                return collections.ToList();
            }

            public static void Add(ThemeCollection theme)
            {
                if(!collections.Exists(th => th.Id == theme.Id))
                {
                    collections.Add(theme);
                }
                else
                {
                    _ = LoggerService.LogAsync(typeof(Collection), "Данная модель уже существует с таким Id\nВозможно theme.json собран неправильно", Message.Warn);
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
                    _ = LoggerService.LogAsync(typeof(Collection), $"Данная модель уже не существует с таким Id {theme.Id}. Возможно theme.json собран неправильно", Message.Warn);
                }
            }

            public static void Clear()
            {
                if(collections == null)
                {
                    _ = LoggerService.LogAsync(typeof(Collection), "Невозможно очистить коллекцию, поскольку она уже пуста!", Message.Warn);
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
            private static string themeImagePath; 

            public static List<Image> GetImagesWithPhase(List<Image> images, Times time)
            {
                return images.Where(image => image.times == time).ToList();
            }

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
                    _ = LoggerService.LogAsync(typeof(ThemeService), $"Число не должно быть равным 0", Message.Warn);
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

                    _ = LoggerService.LogAsync(typeof(ThemeService),
                        $"Тема найдена {theme.Name}");
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
                    if(AppSettingsRepository.AppSettingsService.ExistsFile(imageLocation))
                    {
                        return imageLocation;
                    }
                    else
                    {
                        _ = LoggerService.LogAsync(typeof(ThemeService), "Не удалось найти изображение, переустановите тему", Message.Warn);
                    }
                }
                _ = LoggerService.LogAsync(typeof(ThemeService), "Не удалось найти модель изображения", Message.Warn);
                return null;
            }

            public static string GetCurrentImage(Times themeCollection)
            {
                if (IsImages())
                {
                    var image = GetImagesOrderBy().FirstOrDefault(s => s.times == themeCollection);
                    if (image != null)
                    {
                        themeImageId = image.id;
                        var imageLocation = image.location;
                        if (AppSettingsRepository.AppSettingsService.ExistsFile(imageLocation))
                        {
                            themeImagePath = imageLocation;
                            return imageLocation;
                        }
                    }
                }
                themeImagePath = null;
                _ = LoggerService.LogAsync(typeof(ThemeService), "Не удалось найти модель изображения", Message.Warn);
                return null;
            }

            public static void Disable()
            {
                themeModel = null;
                new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder()
                    .SetTheme(null)
                    .Build();
            }

            /// <summary>
            /// Проверка на раннее утро
            /// </summary>
            /// <param name="nowDateTime"></param>
            /// <returns></returns>
            public static bool IsDawnSolarTime(DateTime nowDateTime)
            {
                var phaseModel = PhaseRepository.Get();

                var date1 = phaseModel.dawnSolarTime;
                var date2 = phaseModel.sunriseSolarTime;

                if (date1 < nowDateTime && date2 > nowDateTime)
                {
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.Dawn);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Проверка на утро
            /// </summary>
            /// <param name="nowDateTime"></param>
            /// <returns></returns>
            public static bool IsSunriseSolarTime(DateTime nowDateTime)
            {
                var phaseModel = PhaseRepository.Get();

                var date1 = phaseModel.sunriseSolarTime;
                var date2 = phaseModel.daySolarTime;

                if (date1 < nowDateTime && date2 > nowDateTime)
                {
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.Sunrise);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Проверка на полдень
            /// </summary>
            /// <param name="nowDateTime"></param>
            /// <returns></returns>
            public static bool IsDaySolarTime(DateTime nowDateTime)
            {
                var phaseModel = PhaseRepository.Get();

                var date1 = phaseModel.daySolarTime;
                var date2 = phaseModel.goldenSolarTime;

                if (date1 < nowDateTime && date2 > nowDateTime)
                {
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.Day);
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Золотой час, солнце опускается
            /// </summary>
            /// <param name="nowDateTime"></param>
            /// <returns></returns>
            public static bool IsGoldenSolarTime(DateTime nowDateTime)
            {
                var phaseModel = PhaseRepository.Get();

                var date1 = phaseModel.goldenSolarTime;
                var date2 = phaseModel.sunsetSolarTime;
                if (date1 < nowDateTime && date2 > nowDateTime)
                {
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.GoldenHour);
                    return true;
                }
                return false;
            }
            public static bool IsSunsetSolarTime(DateTime nowDateTime)
            {
                var phaseModel = PhaseRepository.Get();

                var date1 = phaseModel.sunsetSolarTime;
                var date2 = phaseModel.duskSolarTime;

                if (date1 < nowDateTime && date2 > nowDateTime)
                {
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.Sunset);
                    return true;
                }
                return false;
            }
            public static bool IsDuskSolarTime(DateTime nowDateTime)
            {
                var phaseModel = PhaseRepository.Get();

                var date1 = phaseModel.duskSolarTime;

                if (date1 < nowDateTime)
                {
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.Night);
                    return true;
                }
                return false;
            }

            public static string GetStaticImage()
            {
                return themeImagePath;
            }
        }

        public static void Load()
        {
            try
            {
                var jsonText = File.ReadAllText(ThemeService.GetPathConfig());
                themeModel = Json<Theme>.Decode(jsonText);
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

        public static bool IsImages()
        {
            return Get() != null && themeModel.Images != null;
        }

        public static List<Image> GetImages()
        {
            return themeModel.Images;
        }

        public static List<Image> GetImagesOrderBy()
        {
            return themeModel.Images.OrderBy(image => image.id).ToList();
        }
        public static List<Image> GetImagesOrderBy(Times times)
        {
            return themeModel.Images.OrderBy(image => image.times == times).ToList();
        }

        public static string GetName()
        {
            var value = Get();
            return value?.Name;
        }
    }
}