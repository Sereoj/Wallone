using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WinDynamicDesktop.Core.Extension;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ConfigService
    {
        private static Config config;
        public ConfigService(string path)
        {
            Open(path);
        }
        public static void Open(string path)
        {
            if (path.ExistsFile())
            {
                config = JsonConvert.DeserializeObject<Config>(path.ReadFile());
            }
        }

        public static Config GetConfig()
        {
            return config;
        }
    }
}
