using System;
using System.Collections.ObjectModel;
using System.Linq;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Services;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private ObservableCollection<NavigationViewItem> brands = new ObservableCollection<NavigationViewItem>();

        private ObservableCollection<NavigationViewItem> categories = new ObservableCollection<NavigationViewItem>();

        private int footerHeight;

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

        public ObservableCollection<NavigationViewItem> Brands
        {
            get => brands;
            set => SetProperty(ref brands, value);
        }

        public ObservableCollection<NavigationViewItem> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        public int FooterHeight
        {
            get => footerHeight;
            set => SetProperty(ref footerHeight, value);
        }
        private string text = "This is text";
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public DelegateCommand<NavigationViewItemInvokedEventArgs> MenuItemInvokedCommand { get; set; }

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
            GC.Collect(2);
        }

        private void OnMenuItemInvoked(NavigationViewItemInvokedEventArgs e)
        {
            var text = e.InvokedItemContainer.Content;

            Text = Text;

            var param = new NavigationParameters
            {
                {"Root", e.InvokedItemContainer.Tag.ToString()},
                {"Page", e.InvokedItemContainer.Name},
                {"ID", e.InvokedItemContainer.Uid},
                {"Text", text}
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
                        regionManager.RequestNavigate("PageRegion", "Settings", param);
                    else
                        regionManager.RequestNavigate("PageRegion", "Wallpapers", param);
                    break;
            }
        }

        public async void LoadBrands()
        {
            try
            {
                var items = await BrandsService.GetBrandAsync(null);

                foreach (var item in items.Where(item => item.Status))
                {
                    Brands.Add(new NavigationViewItem
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = item.Tag != null ? item.Tag.ToLower() : "walllpapers",
                        Icon = item.Icon != null ? FontIconService.SetIcon("ultimate", item.Icon) : null,
                        Tag = "Brands"
                    });
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    {"Text", ex.Message}
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }

        public async void LoadCategory()
        {
            try
            {
                var items = await CategoriesService.GetCategoryAsync(null);

                foreach (var item in items.Where(item => item.Status))
                {
                    Categories.Add(new NavigationViewItem
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = item.Tag != null ? item.Tag.ToLower() : "walllpapers",
                        Icon = item.Icon != null ? FontIconService.SetIcon("ultimate", item.Icon) : null,
                        Tag = "Categories"
                    });
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    {"Text", ex.Message}
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}