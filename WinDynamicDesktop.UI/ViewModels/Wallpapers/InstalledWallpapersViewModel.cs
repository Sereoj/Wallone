using Prism.Regions;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class InstalledWallpapersViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string header = "Установленные";
        public string Header { get => header; set => SetProperty(ref header, value); }

        public ObservableCollection<ArticleViewModel> Library { get; set; } = new ObservableCollection<ArticleViewModel>();

        public InstalledWallpapersViewModel()
        {

        }
        public InstalledWallpapersViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
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
