using ModernWpf.Controls;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.SetNavigationView(NavigationView);
            NavigationService.CurrentItemID(0);
            NavigationView.SelectedItem = NavigationService.GetSelectedItem();
        }

        private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if(ActualWidth > 1024)
            {
                NavigationView.IsPaneOpen = true;
            }
            else
            {
                NavigationView.IsPaneOpen = false;
            }
        }
    }
}
