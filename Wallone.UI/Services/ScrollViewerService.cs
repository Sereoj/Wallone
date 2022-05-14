using System.Windows.Controls;

namespace Wallone.UI.Services
{
    public class ScrollViewerService
    {
        private static readonly ScrollData data = new ScrollData();

        public static ScrollData Get(ref ScrollChangedEventArgs e)
        {
            data.offset = e.ViewportHeight + e.VerticalOffset;
            data.offset100 = e.ViewportHeight + 100;
            data.percent80 = e.ExtentHeight / 100 * 80;
            data.percent90 = e.ExtentHeight / 100 * 90;
            data.percent95 = e.ExtentHeight / 100 * 95;
            return data;
        }
    }
}