using System.Windows;

namespace Wallone.UI.Services
{
    public class AppSizeService
    {
        private static Size appSize;
        public static void Set(double h, double w)
        {
            appSize.Height = h;
            appSize.Width = w;
        }

        public static Size Get()
        {
            return appSize;
        }
    }
}