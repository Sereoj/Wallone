using System.IO;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;

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

        public SettingsBuilder UpdateOrCreateFile(string filename)
        {
            var path = Path.Combine(Platformer.GetHelper().GetCurrentFolder(), filename);

            SettingsService.SetFile(path); // Установка пути
            if (!SettingsService.Exist()) // Проверка файла
                CreateFile(path); // Создание файла
            SettingsService.Load(); // Загрузка данных

            AppSettingsService.SetSettingsLocation(path);

            LoggerService.Log(this, path);
            return this;
        }

        public SettingsBuilder SetConfigName(string name)
        {
            AppSettingsService.SetThemeConfigName(name);

            LoggerService.Log(this, name);
            return this;
        }

        public SettingsItemBuilder ItemBuilder()
        {
            return new SettingsItemBuilder(SettingsService.Get());
        }

        public SettingsBuilder Build()
        {
            SettingsService.Save();
            return this;
        }
    }
}