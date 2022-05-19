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
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;

        public void Set(Theme theme)
        {
            if (theme != null)
            {
                ThemeService.Set(theme);

                var filename = GetCurrentImageByTime(theme.Images).location;
                if (filename != null)
                {
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
                }

                ThemeService.Save();
            }
        }

        public Image GetCurrentImageByTime(List<Image> images)
        {
            var hours = int.Parse(DateTime.Now.ToString("HH"));

            if (6 <= hours && hours <= 11)
            {
                return images.FirstOrDefault(image => image.times == "sunrise");
            }

            if (12 <= hours && hours <= 17)
            {
                return images.FirstOrDefault(image => image.times == "day");
            }

            if (18 <= hours && hours <= 23)
            {
                return images.FirstOrDefault(image => image.times == "sunset");
            }

            if (24 >= hours && hours <= 6)
            {
                return images.FirstOrDefault(image => image.times == "night");
            }

            return null;
        }
    }
}