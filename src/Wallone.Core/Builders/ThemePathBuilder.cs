using System.IO;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Builders
{
    public class ThemePathBuilder : IAppSettings
    {
        public ThemePathBuilder CreateDirectory(string path)
        {
            AppSettingsRepository.AppSettingsService.CreateDirectory(path);
            return this;
        }

        public ThemePathBuilder ExistOrCreateDirectory(string path)
        {
            var appLocation = AppSettingsRepository.AppSettingsService.GetAppLocation();
            var pathThemeDirectory = Path.Combine(appLocation, path);

            if (!AppSettingsRepository.AppSettingsService.ExistDirectory(pathThemeDirectory)) CreateDirectory(pathThemeDirectory);

            AppSettingsRepository.AppSettingsService.SetThemesLocation(pathThemeDirectory);

            LoggerService.Log(this, $"Расположение папки с темами {pathThemeDirectory}");
            return this;
        }

        public ThemePathBuilder UseForFolders(string patten)
        {
            AppSettingsRepository.AppSettingsService.SetThemeNameForFolders(patten);
            LoggerService.Log(this, $"Поиск тем по шаблону {patten}");
            return this;
        }

        public ThemePathBuilder Build()
        {
            return this;
        }
    }
}