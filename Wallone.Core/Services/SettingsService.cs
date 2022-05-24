using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Wallone.Core.Extension;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models.App;

namespace Wallone.Core.Services
{
    public class SettingsService
    {
        private static string file;
        private static ISettings Settings { get; set; }

        public static void SetModel(ISettings settings)
        {
            Settings = settings;
        }

        public static void SetFile(string path)
        {
            file = path;
        }

        public static bool Exist()
        {
            return file.ExistsFile();
        }

        //Ручное сохранение
        public static void Save()
        {
            Settings.Information = "!!Это файл настроек, пожалуйста не удаляйте и не изменяйте его!!";

            File.WriteAllText(file, JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }

        //Загрузка конфига, выполняется один раз
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

        public static string GetToken()
        {
            return Settings.User.Token;
        }

        public static ISettings Get()
        {
            return Settings;
        }
    }
}