using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WinDynamicDesktop.Core.Models.App;

namespace WinDynamicDesktop.Authorization.Services
{
    public class SettingsService
    {
        private static Settings settings = new Settings();
        private static readonly string file = "app.settings";

        public static Settings FileReader(string path)
        {
            settings = path != null
                ? JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path))
                : JsonConvert.DeserializeObject<Settings>(File.ReadAllText(file));
            return settings;
        }
        public static Settings Get()
        {
            return settings;
        }

        private static void Save()
        {
            File.WriteAllText("app.settings", JsonConvert.SerializeObject(settings));
        }
    }
}
