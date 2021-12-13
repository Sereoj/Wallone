using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WinDynamicDesktop.Core.Services
{
    public class CategoriesService
    {
        private static ObservableCollection<NavigationViewItem> items = new ObservableCollection<NavigationViewItem>();
        public static void SetCategory(NavigationViewItem item)
        {
            items.Add(item);
        }
        public static ObservableCollection<NavigationViewItem> GetCategories()
        {
            Default();
            return items;
        }

        public static void Default()
        {
        }
    }
}
