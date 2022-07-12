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
            SettingsRepository.Init(settings);
        }

        public SettingsBuilder CreateFile(string path)
        {
            SettingsRepository.SetFilename(path);
            SettingsRepository.Save();
            return this;
        }

        public SettingsBuilder UpdateOrCreateFile(string filename)
        {
            var path = Path.Combine(Platformer.GetHelper().GetCurrentFolder(), filename);

            SettingsRepository.SetFilename(path); // Установка пути
            if (!SettingsRepository.SettingsService.Exist()) // Проверка файла
                CreateFile(path); // Создание файла
            SettingsRepository.Load(); // Загрузка данных

            AppSettingsRepository.AppSettingsService.SetSettingsLocation(path);

            LoggerService.SysLog(this, $"Расположение настроек: {path}");
            return this;
        }

        public SettingsBuilder SetConfigName(string name)
        {
            AppSettingsRepository.AppSettingsService.SetThemeConfigName(name);

            LoggerService.SysLog(this, $"Используемый конфиг - {name} для чтения тем");
            return this;
        }

        public SettingsItemBuilder ItemBuilder()
        {
            return new SettingsItemBuilder(SettingsRepository.Get());
        }

        public SettingsBuilder Build()
        {
            SettingsRepository.Save();
            return this;
        }
    }
}