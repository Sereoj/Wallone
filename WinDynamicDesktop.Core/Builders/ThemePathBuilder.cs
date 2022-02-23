using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
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
            string appLocation = AppSettingsService.GetAppLocation();
            string pathThemeDirectory = Path.Combine(appLocation, path);


            if (!AppSettingsService.ExistDirectory(pathThemeDirectory))
            {
                CreateDirectory(pathThemeDirectory);
            }
            return this;
        }

        public ThemePathBuilder UseForFolders(string patten)
        {
            AppSettingsService.SetThemeNameForFolders(patten);

            return this;
        }

        public ThemePathBuilder Build()
        {
            return this;
        }
    }
}
