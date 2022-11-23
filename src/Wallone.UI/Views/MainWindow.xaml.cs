using System;
using System.ComponentModel;
using System.Windows;
using Wallone.UI.Services;


namespace Wallone.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
                AppContext.Show(true);
                AppContext.ShowPopup("Приложение работает в фоновом режиме");
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AppContext.Close();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sizeNew = e.NewSize;
            AppSizeService.Set(sizeNew.Height, sizeNew.Width);
        }
    }
}