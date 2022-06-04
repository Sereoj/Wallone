using ModernWpf;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.UI.Properties;

namespace Wallone.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private string title = Common.Translation.Localization.AppName;

        private ModernWpf.ElementTheme currentTheme;
        public ModernWpf.ElementTheme CurrentTheme { get => currentTheme; set => SetProperty(ref currentTheme, value); }

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            currentTheme = ElementTheme.Default;
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            regionManager.RequestNavigate("ContentRegion", "Load");
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