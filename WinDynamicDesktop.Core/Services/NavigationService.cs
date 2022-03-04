using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Core.Services
{
    public class NavigationService
    {
        private static NavigationView NavigationView { get; set; }
        private static NavigationViewItem CurrentItem { get; set; }
        public static void SetNavigationView(NavigationView navigationView)
        {
            NavigationView = navigationView;

        }
        public static void CurrentItemID(int v)
        {
            if (NavigationView.MenuItems != null)
            {
                CurrentItem = NavigationView.MenuItems[v] as NavigationViewItem;
            }
        }

        public static NavigationViewItem GetSelectedItem()
        {
            return CurrentItem;
        }
    }
}
