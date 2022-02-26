using System;
using WinDynamicDesktop.Core.Builders;

namespace WinDynamicDesktop.Core.Controllers
{
    public class ThemeController
    {
        private readonly ThemeCreatedBuilder themeBuilder;

        public ThemeController(ThemeCreatedBuilder themeBuilder)
        {
            this.themeBuilder = themeBuilder;
        }

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
    }
}