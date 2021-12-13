using ModernWpf.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using WinDynamicDesktop.Core.Events;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private ObservableCollection<NavigationViewItem> categories;
        public ObservableCollection<NavigationViewItem> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }

        private int footerHeight = 0;   
        public int FooterHeight
        {
            get { return footerHeight; }
            set { SetProperty(ref footerHeight, value); }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        public DelegateCommand<NavigationViewItemInvokedEventArgs> MenuItemInvokedCommand { get; set; }
        public DelegateCommand<ScrollChangedEventArgs> ScrollViewerScrollChangedCommand { get; set; }

        public MainViewModel()
        {
            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
            Categories = new ObservableCollection<NavigationViewItem>();
        }
        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
            ScrollViewerScrollChangedCommand = new DelegateCommand<ScrollChangedEventArgs>(OnScrollChanged);
            Categories = CategoriesService.GetCategories();
        }

        private void OnScrollChanged(ScrollChangedEventArgs e)
        {
            eventAggregator.GetEvent<ScrollEvent>().Publish(e);
            if (e.VerticalOffset != 0)
            {
                if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
                {
                    FooterHeight = 40;
                }
            }
        }

        private void OnMenuItemInvoked(NavigationViewItemInvokedEventArgs e)
        {
            switch (e.InvokedItemContainer.Tag.ToString())
            {
                case "Library":
                    regionManager.RequestNavigate("PageRegion", "Wallpapers");
                    break;
                case "New":
                    regionManager.RequestNavigate("PageRegion", "WallpapersNew");
                    break;
                case "Popular":
                    regionManager.RequestNavigate("PageRegion", "WallpapersPopular");
                    break;
                case "Wait":
                    regionManager.RequestNavigate("PageRegion", "WallpapersWait");
                    break;
                default:
                    regionManager.RequestNavigate("PageRegion", "Wallpapers");
                    break;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
