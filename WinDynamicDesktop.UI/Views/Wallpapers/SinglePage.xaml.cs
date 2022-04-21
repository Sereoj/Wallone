﻿using System.Windows.Controls;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.Views.Wallpapers
{
    /// <summary>
    /// Логика взаимодействия для SimplePage.xaml
    /// </summary>
    public partial class SinglePage : UserControl
    {
        public SinglePage()
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
