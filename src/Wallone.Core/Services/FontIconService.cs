using System.Windows;
using System.Windows.Media;
using ModernWpf.Controls;

namespace Wallone.Core.Services
{
    public class FontIconService
    {
        public static FontIcon SetIcon(string font, string value)
        {
            FontFamily fontFamily = font switch
            {
                "free" => (FontFamily)Application.Current.Resources["FontIconMoonFree"],
                "icomoon" => (FontFamily)Application.Current.Resources["FontIconMoon"],
                "ultimate" => (FontFamily)Application.Current.Resources["FontIconMoonUltimate"],
                _ => null
            };

            return value == null ? null : new FontIcon {FontFamily = fontFamily, Glyph = value};
        }
    }
}