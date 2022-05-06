using System.IO;
using Wallone.Core.Interfaces;
using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    public class SettingsBuilder : IAppSettings
    {

        public SettingsBuilder(ISettings settings)
        {
            SettingsService.SetModel(settings);
        }

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
                CreateFile(path); // Создание файла
            SettingsService.Load(); // Загрузка данных

            var settingsPath = Path.Combine(AppSettingsService.GetAppLocation(), path);
            AppSettingsService.SetSettingsLocation(settingsPath);
            return this;
        }

        public SettingsBuilder SetConfigName(string name)
        {
            AppSettingsService.SetThemeConfigName(name);
            return this;
        }

        public SettingsItemBuilder ItemBuilder()
        {
            return new SettingsItemBuilder(SettingsService.Get());
            ;
        }

        public SettingsBuilder Build()
        {
            SettingsService.Save();
            return this;
        }
    }
}