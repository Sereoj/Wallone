using System.Windows;
using System.Windows.Controls;
using Wallone.UI.Services;

namespace Wallone.UI.Views.Wallpapers
{
    /// <summary>
    ///     Логика взаимодействия для SimplePage.xaml
    /// </summary>
    public partial class SinglePage : UserControl
    {
        public SinglePage()
        {
            InitializeComponent();
        }

        private void ScrollUp_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToTop();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var data = ScrollViewerService.Get(ref e);

            if (data.offset < data.offset100) ScrollUp.Visibility = Visibility.Hidden;

            if (data.percent80 < data.offset && data.percent90 > data.offset) ScrollUp.Visibility = Visibility.Visible;

            if (data.percent90 < data.offset && data.percent95 > data.offset)
            {
                //TODO
            }

            //Конец
            if (e.ViewportHeight + e.VerticalOffset == e.ExtentHeight)
            {
                //TODO
            }
        }
    }
}