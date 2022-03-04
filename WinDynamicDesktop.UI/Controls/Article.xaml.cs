using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WinDynamicDesktop.Core.Helpers;

namespace WinDynamicDesktop.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для Article.xaml
    /// </summary>
    public partial class Article : UserControl
    {
        public Article()
        {
            InitializeComponent();
        }

        private void BorderAction_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Header.Opacity = 1;
        }

        private void BorderAction_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Header.Opacity = 0;
        }

    }
}
