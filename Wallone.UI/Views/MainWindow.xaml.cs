using System;
using System.ComponentModel;
using System.Windows;


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
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AppContext.Close();
        }
    }
}