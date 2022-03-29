using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private ObservableCollection<NavigationViewItem> brands = new ObservableCollection<NavigationViewItem>();
        public ObservableCollection<NavigationViewItem> Brands
        {
            get { return brands; }
            set { SetProperty(ref brands, value); }
        }

        private ObservableCollection<NavigationViewItem> categories = new ObservableCollection<NavigationViewItem>();
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
        public MainViewModel()
        {
        }
        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            LoadBrands();
            LoadCategory();

            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
        }

        private void OnMenuItemInvoked(NavigationViewItemInvokedEventArgs e)
        {
            var text = e.InvokedItemContainer.Content;

            var param = new NavigationParameters
                    {
                        { "Root", e.InvokedItemContainer.Tag.ToString() },
                        { "Page", e.InvokedItemContainer.Name.ToString() },
                        { "ID", e.InvokedItemContainer.Uid.ToString() },
                        {"Text", text }
                    };

            switch (e.InvokedItemContainer.Tag.ToString())
            {
                case "Profile":
                    regionManager.RequestNavigate("PageRegion", "Profile", param);
                    break;
                case "Account":
                    regionManager.RequestNavigate("PageRegion", "Account", param);
                    break;
                default:

                    if (e.IsSettingsInvoked)
                    {
                        regionManager.RequestNavigate("PageRegion", "Settings", param);
                    }
                    else
                    {
                        regionManager.RequestNavigate("PageRegion", "Wallpapers", param);
                    }
                    break;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            regionManager.RequestNavigate("PageRegion", "Wallpapers");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public async void LoadBrands()
        {
            try
            {
                var items = await BrandsService.GetBrandAsync(null);

                foreach (var item in items)
                {
                    Brands.Add(new NavigationViewItem()
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = item.Tag.ToLower(),
                        Icon = FontIconService.SetIcon("ultimate", item.Icon),
                        Tag = "Brands"
                    });
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
        public async void LoadCategory()
        {
            try
            {
                var items = await CategoriesService.GetCategoryAsync(null);

                foreach (var item in items)
                {
                    Categories.Add(new NavigationViewItem()
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = item.Tag.ToLower(),
                        Icon = FontIconService.SetIcon("ultimate", item.Icon),
                        Tag = "Categories"
                    });
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}
