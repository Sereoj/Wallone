using System;
using System.IO;
using Newtonsoft.Json;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services.App;

namespace Wallone.Core.Services
{
    public class ThemeRepository
    {
        private static Theme themeModel;

        public class ThemeService
        {
            private static string themeName;

            public static string GetCurrentName()
            {
                return themeName ?? "default";
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