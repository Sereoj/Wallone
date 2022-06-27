using System;
using System.IO;
using Newtonsoft.Json;
using Wallone.Core.Extension;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models.App;

namespace Wallone.Core.Services.App
{
    public class SettingsRepository
    {
        private static ISettings Settings { get; set; }
        private static string file;

        public static void Init(ISettings settings)
        {
            if (settings != null)
            {
                Settings = settings;
            }
        }

        public static void SetFilename(string path)
        {
            file = !string.IsNullOrEmpty(path) ? path : "app.settings";
        }

        public static void Save()
        {
            Settings.Information = "!!Это файл настроек, пожалуйста не удаляйте и не изменяйте его!!";

            File.WriteAllText(file, JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }

        public static void Load()
        {
            try
            {
                var jsonText = File.ReadAllText(file);
                if (JsonHelper.IsValidJson(jsonText)) Settings = JsonConvert.DeserializeObject<Settings>(jsonText);
            }
            catch (Exception ex)
            {
            }
        }

        public class SettingsService
        {
            public static bool Exist()
            {
                return file.ExistsFile();
            }
        }

        public static ISettings Get()
        {
            return Settings;
        }
    }
}