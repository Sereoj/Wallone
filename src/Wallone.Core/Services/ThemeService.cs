using System;
using System.IO;
using Newtonsoft.Json;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services.App;

namespace Wallone.Core.Services
{
    public class ThemeService
    {
        private static Theme themeModel;
        private static string themeName;
        private static TimeSpan spanImage;

        public static string GetCurrentName()
        {
            return themeName ?? "default";
        }

        public static void SetCurrentName(string name)
        {
            themeName = AppFormat.Format(name);

            new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .SetTheme(name)
                .Build();
        }

        public static string GetPath()
        {
            return Path.Combine(AppSettingsService.GetThemesLocation(), themeName);
        }

        public static string GetPathConfig()
        {
            return Path.Combine(GetPath(), AppSettingsService.GetThemeConfigName());
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

        public static void SetTimeSpan(TimeSpan span)
        {
            spanImage = span;
        }

        public static TimeSpan GetTimeSpan()
        {
            return spanImage;
        }
        public static void Load()
        {
            try
            {
                var jsonText = File.ReadAllText(GetPathConfig());
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
                var file = GetPathConfig();
                File.WriteAllText(file, JsonConvert.SerializeObject(themeModel, Formatting.Indented));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}