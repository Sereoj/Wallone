using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.Core.Controllers
{
    public class ThemeController
    {
        private static readonly uint SPI_SETDESKWALLPAPER = 20;
        private static readonly uint SPIF_UPDATEINIFILE = 0x1;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        enum TimeOfDay
        {
           TimeSet, // пользователь может сам настроить время для смены
           Auto, // Автоматически подбираются данные за счет геолокации.
        }

        public void Set(Theme theme)
        {
            if (theme != null)
            {
                ThemeService.Set(theme);

                var filename = GetCurrentImageByTime(theme.Images).location;
                if (filename != null)
                    if (AppSettingsService.ExistsFile(filename))
                        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
                ThemeService.Save();
            }
        }


        public Image GetCurrentImageByTime(List<Image> images)
        {
            var hours = int.Parse(DateTime.Now.ToString("HH"));

            if (6 <= hours && hours <= 11) return images.FirstOrDefault(image => image.times == "sunrise");

            if (12 <= hours && hours <= 17) return images.FirstOrDefault(image => image.times == "day");

            if (18 <= hours && hours <= 23) return images.FirstOrDefault(image => image.times == "sunset");

            if (24 >= hours && hours <= 6) return images.FirstOrDefault(image => image.times == "night");

            return null;
        }
    }
}