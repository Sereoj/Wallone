using ModernWpf.Controls;
using System.Windows;
using System.Windows.Media;

namespace Wallone.UI.Services
{
    public class FontIconService
    {
        public static FontIcon SetIcon(string font, string value)
        {
            FontFamily fontFamily = null;

            switch (font)
            {
                case "free":
                    fontFamily = (FontFamily)Application.Current.Resources["FontIconMoonFree"];
                    break;
                case "icomoon":
                    fontFamily = (FontFamily)Application.Current.Resources["FontIconMoon"];
                    break;
                case "ultimate":
                    fontFamily = (FontFamily)Application.Current.Resources["FontIconMoonUltimate"];
                    break;
            }

            return new FontIcon { FontFamily = fontFamily, Glyph = value };
        }
    }
}