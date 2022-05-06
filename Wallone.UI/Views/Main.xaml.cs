using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;
using Wallone.Core.Services;

namespace Wallone.UI.Views
{
    /// <summary>
    ///     Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();
            NavigationView.IsPaneOpen = false;
            NavigationView.OpenPaneLength = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.SetNavigationView(NavigationView);
            NavigationService.CurrentItemID(0);
            NavigationView.OpenPaneLength = 240;
            NavigationView.SelectedItem = NavigationService.GetSelectedItem();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth <= 680)
            {
                NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
            }
            else if (ActualWidth >= 680 && ActualWidth < 800)
            {
                if (ActualHeight <= 550)
                {
                    NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
                }
                else
                {
                    NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
                    NavigationView.IsPaneOpen = false;
                }
            }
            else if (ActualWidth >= 800 && ActualWidth < 1024)
            {
                if (ActualHeight <= 550)
                {
                    NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
                }
                else
                {
                    NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
                    NavigationView.IsPaneOpen = false;
                }
            }
            else if (ActualWidth > 1024)
            {
                if (ActualHeight <= 550)
                {
                    NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
                }
                else
                {
                    NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
                    NavigationView.IsPaneOpen = true;
                }
            }
        }
    }
}