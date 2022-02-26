using Newtonsoft.Json;
using System;
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
        private static Settings Settings { get; set; } = new Settings();
        private static string file;
        private static Timer autoSaveTimer;
        private static bool restartPending = false;
        private static bool unsavedChanges;

        public static void SetModel(Settings settings)
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
            File.WriteAllText(file, JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }
        //Проверка на первый запуск
        public static bool CheckFirstLaunch()
        {
            Trace.WriteLine("Проверка на первый запуск");
            return !file.ExistsFile();
        }
        //Загрузка конфига, выполняется один раз
        public static void Load()
        {
            if (autoSaveTimer != null)
            {
                autoSaveTimer.Stop();
            }

            try
            {
                string jsonText = File.ReadAllText(file);
                Settings = JsonConvert.DeserializeObject<Settings>(jsonText);
            }
            catch(Exception ex)
            {

            }

            unsavedChanges = false;
            autoSaveTimer = new Timer
            {
                AutoReset = false,
                Interval = 100
            };

            Settings.PropertyChanged += OnSettingsPropertyChanged;
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
            return Settings;
        }
    }
}
