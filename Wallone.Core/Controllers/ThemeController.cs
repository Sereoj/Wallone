using System;
using System.IO;
using System.Runtime.InteropServices;
using Wallone.Core.Builders;

namespace Wallone.Core.Controllers
{
    public class ThemeController
    {
        private readonly ThemeCreatedBuilder themeBuilder;

        public ThemeController(ThemeCreatedBuilder themeBuilder)
        {
            this.themeBuilder = themeBuilder;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public bool GetValueInstall()
        {
            return themeBuilder.GetHasNotInstalled();
        }

        public bool GetValueFavorite()
        {
            return themeBuilder.GetHasNotFavorited();
        }

        public bool GetValueReaction()
        {
            return themeBuilder.GetHasNotLiked();
        }

        private void SetWallpaper(string WallpaperLocation, int WallpaperStyle, int TileWallpaper)
        {
            SystemParametersInfo(20, 0, WallpaperLocation, 0x01 | 0x02);
        }

        public async void SetWallpaper()
        {
            try
            {
                if (themeBuilder.GetHasNotInstalled())
                {
                    var files = Directory.GetFiles(themeBuilder.GetThemePath());
                    SetWallpaper(files[0], 2, 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}