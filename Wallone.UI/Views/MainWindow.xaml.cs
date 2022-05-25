using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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
            if (this.WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
                AppContext.Show(true);
            }
        }
    }
}