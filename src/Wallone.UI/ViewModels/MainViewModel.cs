using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Services;
using Wallone.Core.Services.App;

namespace Wallone.UI.ViewModels
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public ManagerViewModel ManagerViewModel { get; }

        private ObservableCollection<NavigationViewItem> categories = new ObservableCollection<NavigationViewItem>();

        public ObservableCollection<NavigationViewItem> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        public DelegateCommand<NavigationViewItemInvokedEventArgs> MenuItemInvokedCommand { get; set; }

        public MainViewModel(){}

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            Task.Run(async () => await AppService.LoadVersionAsync());
            Task.Run(async () => await AppService.UseGeolocationAsync());
            AppService.UseTheme();

            var builder = new UserSyncBuilder()
                .CreateUserModel()
                .GetToken()
                .ValidateAsync();
            LoadCategory();

            ManagerViewModel = new ManagerViewModel(regionManager);

            MenuItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnMenuItemInvoked);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var param = new NavigationParameters
            {
                {"Root", "Gallery"},
                {"Page", ""},
                {"ID", "Main"},
                {"Text", "Библиотека"}
            };
            regionManager.RequestNavigate("PageRegion", "Wallpapers", param);
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
            var manager = new ManagerViewModel(regionManager);
            manager.Open(e);
        }

        public void LoadCategory() => Categories = CategoriesService.LoadCategories();
    }
}