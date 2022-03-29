using ModernWpf.Controls;
using System.Windows.Media;

namespace WinDynamicDesktop.UI.Services
{
    public class FontIconService
    {
        public static FontIcon SetIcon(string font, string value)
        {
            FontFamily fontFamily = null;

            switch (font)
            {
                case "free":
                    fontFamily = (FontFamily)App.Current.Resources["FontIconMoonFree"];
                    break;
                case "icomoon":
                    fontFamily = (FontFamily)App.Current.Resources["FontIconMoon"];
                    break;
                case "ultimate":
                    fontFamily = (FontFamily)App.Current.Resources["FontIconMoonUltimate"];
                    break;
            }
            return new FontIcon() { FontFamily = fontFamily, Glyph = value };
        }
    }
}
