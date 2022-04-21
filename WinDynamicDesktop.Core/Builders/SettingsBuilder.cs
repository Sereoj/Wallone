using System.IO;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class SettingsBuilder : IAppSettings
    {
        public SettingsBuilder CreateFile(string path)
        {
            SettingsService.SetFile(path);
            SettingsService.Save();
            return this;
        }
        public SettingsBuilder UpdateOrCreateFile(string path)
        {
            SettingsService.SetFile(path); // Установка пути
            if (!SettingsService.Exist()) // Проверка файла
            {
                CreateFile(path); // Создание файла
            }
            SettingsService.Load(); // Загрузка данных

            var settingsPath = Path.Combine(AppSettingsService.GetAppLocation(), path);
            AppSettingsService.SetSettingsLocation(settingsPath);
            return this;
        }

        public SettingsItemBuiler Items()
        {
            return new SettingsItemBuiler();
        }

        public SettingsBuilder Build()
        {
            SettingsService.Save();
            return this;
        }
    }
}
