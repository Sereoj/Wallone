using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wallone.UI.Services;

namespace Wallone.UI.Views.Wallpapers
{
    /// <summary>
    ///     Логика взаимодействия для Wallpapers.xaml
    /// </summary>
    public partial class Wallpapers : UserControl
    {
        public Wallpapers()
        {
            InitializeComponent();
        }

        private void ScrollUp_Click(object sender, RoutedEventArgs e)
        {
           // ScrollViewer.ScrollToTop();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Trace.WriteLine("H: " + ActualHeight);
            Trace.WriteLine("W: " + ActualWidth);
        }
    }
}