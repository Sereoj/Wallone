using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для InstalledWallpapers.xaml
    /// </summary>
    public partial class InstalledWallpapers : UserControl
    {
        public InstalledWallpapers()
        {
            InitializeComponent();
        }

        private void ScrollUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ScrollViewer.ScrollToTop();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollData data = ScrollViewerService.Get(ref e);

            if (data.offset < data.offset100)
            {
                ScrollUp.Visibility = System.Windows.Visibility.Hidden;
            }

            if (data.percent80 < data.offset && data.percent90 > data.offset)
            {
                ScrollUp.Visibility = System.Windows.Visibility.Visible;
            }

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
