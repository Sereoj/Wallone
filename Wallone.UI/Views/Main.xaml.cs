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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.SetNavigationView(NavigationView);
            NavigationService.CurrentItemID(0);
            NavigationView.SelectedItem = NavigationService.GetSelectedItem();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth > 1024)
                NavigationView.IsPaneOpen = true;
            else
                NavigationView.IsPaneOpen = false;
        }
    }
}