using System.Windows.Controls;

namespace WinDynamicDesktop.UI.Services
{
    public class ScrollData
    {
        public double offset { get; set; }
        public double offset100 { get; set; }
        public double percent80 { get; set; }
        public double percent90 { get; set; }
        public double percent95 { get; set; }
    }
    class ScrollViewerService
    {
        private static ScrollData data = new ScrollData();
        internal static ScrollData Get(ref ScrollChangedEventArgs e)
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
