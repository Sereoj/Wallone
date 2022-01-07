using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using WinDynamicDesktop.Core.Extension;
using WinDynamicDesktop.Core.Models.App;

namespace WinDynamicDesktop.Core.Services
{
    public class SettingsService
    {
        private static Settings settings = new Settings();
        public static bool firstRun;
        private static readonly string file = "app.settings";
        private static readonly Timer autoSaveTimer = new Timer();

        private static void Save()
        {
            File.WriteAllText("app.settings", JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
        public static bool CheckFirstLaunch()
        {
            return firstRun = file.ExistsFile();
        }
        public static void Load()
        {
            if(autoSaveTimer.Enabled)
            {
                autoSaveTimer.Stop();
            }

            if(CheckFirstLaunch())
            {
                settings = new Settings();
            }

            autoSaveTimer.AutoReset = false;
            autoSaveTimer.Interval = 1000;

            settings.PropertyChanged += OnSettingsPropertyChanged;
            autoSaveTimer.Elapsed += OnAutoSaveTimerElapsed;
        }
        private static void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            autoSaveTimer.Start();
        }
        private async static void OnAutoSaveTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                Save();
            });
        }

        public static Settings Get()
        {
            return settings;
        }
    }
}
