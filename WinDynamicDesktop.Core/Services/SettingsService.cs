using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
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
        private static readonly string file = "app.settings";
        private static Timer autoSaveTimer;
        private static bool restartPending = false;
        private static bool unsavedChanges;

        //Ручное сохранение
        public static void Save()
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
        //Проверка на первый запуск
        public static bool CheckFirstLaunch()
        {
            Trace.WriteLine("Проверка на первый запуск");
            return file.ExistsFile();
        }
        //Загрузка конфига, выполняется один раз
        public static void Load()
        {
            if(autoSaveTimer != null)
            {
                autoSaveTimer.Stop();
            }

            if(CheckFirstLaunch())
            {
                settings = new Settings();
            }

            unsavedChanges = false;
            autoSaveTimer = new Timer();
            autoSaveTimer.AutoReset = false;
            autoSaveTimer.Interval = 100;

            settings.PropertyChanged += OnSettingsPropertyChanged;
            autoSaveTimer.Elapsed += OnAutoSaveTimerElapsed;
        }
        
        private static void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Trace.WriteLine("Файл настроек изменен");
            unsavedChanges = true;
            autoSaveTimer.Start();
        }
        private async static void OnAutoSaveTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!restartPending && !unsavedChanges)
            {
                return;
            }

            if (unsavedChanges)
            {
                unsavedChanges = false;
                autoSaveTimer.Elapsed -= OnAutoSaveTimerElapsed;
                
                await Task.Run(() =>
                {
                    Save();
                    Trace.WriteLine("Автоматическое сохранение");
                });
            }

            if (restartPending)
            {
                restartPending = false;
            }
            else
            {
                autoSaveTimer.Elapsed += OnAutoSaveTimerElapsed;
                autoSaveTimer.Start();
            }
        }

        public static Settings Get()
        {
            return settings;
        }
    }
}
