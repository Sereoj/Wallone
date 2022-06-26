﻿using System.IO;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Builders
{
    public class ThemePathBuilder : IAppSettings
    {
        public ThemePathBuilder CreateDirectory(string path)
        {
            AppSettingsService.CreateDirectory(path);
            return this;
        }

        public ThemePathBuilder ExistOrCreateDirectory(string path)
        {
            var appLocation = AppSettingsService.GetAppLocation();
            var pathThemeDirectory = Path.Combine(appLocation, path);

            if (!AppSettingsService.ExistDirectory(pathThemeDirectory)) CreateDirectory(pathThemeDirectory);

            AppSettingsService.SetThemesLocation(pathThemeDirectory);

            LoggerService.Log(this, pathThemeDirectory);
            return this;
        }

        public ThemePathBuilder UseForFolders(string patten)
        {
            AppSettingsService.SetThemeNameForFolders(patten);
            LoggerService.Log(this, patten);
            return this;
        }

        public ThemePathBuilder Build()
        {
            return this;
        }
    }
}